const express = require("express");
const bodyParser = require("body-parser");
const shortUrl = require("shorten-url");

const app = express();
const PORT = process.env.PORT || 3000;

app.use(bodyParser.urlencoded({ extended: true }));
app.use(bodyParser.json());
app.use(express.static("public"));

app.post("/api/shorten", (req, res) => {
  const { url } = req.body;
  const urlReturn = shortUrl(url, 200);
  res.json({ shortURL: `${urlReturn}` });
});

app.listen(PORT, () => {
  console.log(`Server is running on http://localhost:${PORT}`);
});
