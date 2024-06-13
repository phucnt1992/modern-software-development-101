const express = require("express");
const app = express();
const port = 8000;

const apiRouter = express.Router();

apiRouter.get("/liveness", (req, res) => {
  res.status(200).send("OK");
});

apiRouter.get("/readiness", (req, res) => {
  res.status(200).send("OK");
});

app.use("/api/_healthz", apiRouter);

// Start the server
app.listen(port, () => {
  console.log(`Server is running on http://localhost:${port}`);
});
