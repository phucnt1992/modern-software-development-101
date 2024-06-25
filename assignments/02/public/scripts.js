document.addEventListener("DOMContentLoaded", () => {
  const shortenForm = document.getElementById("shortenForm");
  const shortenedUrlDiv = document.getElementById("shortenedUrl");

  shortenForm.addEventListener("submit", async (e) => {
    e.preventDefault();
    const urlInput = document.getElementById("urlInput").value;

    try {
      const response = await fetch("/api/shorten", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ url: urlInput }),
      });

      if (!response.ok) {
        throw new Error("Failed to shorten URL");
      }

      const result = await response.json();
      shortenedUrlDiv.innerHTML = `<a href="${result.shortURL}" target="_blank">${result.shortURL}</a>`;
    } catch (error) {
      console.error("Error:", error);
    }
  });
});
