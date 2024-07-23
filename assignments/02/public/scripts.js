document.addEventListener("DOMContentLoaded", () => {
  const shortenForm = document.getElementById("shortenForm");
  const shortenedUrlDiv = document.getElementById("shortenedUrl");

  const shortenUrl = async (url) => {
    try {
      const response = await fetch("/api/shorten", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ url }),
      });

      if (!response.ok) {
        throw new Error("Failed to shorten URL");
      }

      return await response.json();
    } catch (error) {
      console.error("Error shortening URL:", error);
    }
  };

  const handleShortenFormSubmit = async (e) => {
    e.preventDefault();
    const urlInput = document.getElementById("urlInput").value;

    const result = await shortenUrl(urlInput);
    if (result) {
      shortenedUrlDiv.innerHTML = `<a href="http://${result.shortURL}" target="_blank">${result.shortURL}</a>`;
    }
  };

  shortenForm.addEventListener("submit", handleShortenFormSubmit);
});
