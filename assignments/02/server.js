const express = require("express");
const bodyParser = require("body-parser");
const generateUniqueId = require("generate-unique-id");
const { Pool } = require("pg");
const cors = require("cors");

const app = express();
const PORT = process.env.PORT || 3000;

const pool = new Pool({
  user: "postgres",
  host: "localhost",
  database: "url_shortener",
  password: "123",
  port: 5432,
});

app.use(bodyParser.urlencoded({ extended: true }));
app.use(bodyParser.json());
app.use(express.static("public"));
app.use(cors());

app.post("/api/shorten", async (req, res) => {
  try {
    const { url: originalUrl } = req.body;
    const client = await pool.connect();
    const existingUrlResult = await client.query(
      "SELECT * FROM urls WHERE original_url = $1",
      [originalUrl]
    );
    if (existingUrlResult.rows.length > 0) {
      client.release();
      res.status(200).json({
        originalUrl,
        shortURL: existingUrlResult.rows[0].shortened_url,
      });
    } else {
      const shortURL = generateUniqueId();
      const result = await client.query(
        "INSERT INTO urls (original_url, shortened_url) VALUES ($1, $2) RETURNING *",
        [originalUrl, `${req.headers.host}/${shortURL}`]
      );
      client.release();
      res
        .status(201)
        .json({ originalUrl, shortURL: result.rows[0].shortened_url });
    }
  } catch (error) {
    console.error("Error executing query", error);
    res.status(500).json({ error: "Internal server error" });
  }
});

app.get("/:shortURL", async (req, res) => {
  try {
    const { shortURL } = req.params;
    const client = await pool.connect();
    const result = await client.query(
      "SELECT original_url FROM urls WHERE shortened_url = $1",
      [`${req.headers.host}/${shortURL}`]
    );
    client.release();
    if (result.rows.length > 0) {
      const originalUrl = result.rows[0].original_url;
      res.redirect(301, originalUrl);
    } else {
      res.status(404).json({ error: "Short URL not found" });
    }
  } catch (error) {
    console.error("Error executing query", error);
    res.status(500).json({ error: "Internal server error" });
  }
});

app.listen(PORT, () => {
  console.log(`Server is running on http://localhost:${PORT}`);
});
