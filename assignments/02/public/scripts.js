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

  const fetchOriginalUrl = async (shortUrl) => {
    try {
      const shortUrlPath = shortUrl.split("/").pop();
      const response = await fetch(`/${shortUrlPath}`);

      if (!response.ok) {
        throw new Error("Failed to fetch original URL");
      }

      return await response.json();
    } catch (error) {
      console.error("Error fetching original URL:", error);
    }
  };

  const handleShortenFormSubmit = async (e) => {
    e.preventDefault();
    const urlInput = document.getElementById("urlInput").value;

    const result = await shortenUrl(urlInput);
    if (result) {
      shortenedUrlDiv.innerHTML = `<a class='url' href="${result.shortURL}" target="_blank">${result.shortURL}</a>`;
      addClickListenerToShortenedUrl();
    }
  };

  const handleShortUrlClick = async (event) => {
    event.preventDefault();
    const shortUrl = event.target.href;

    const result = await fetchOriginalUrl(shortUrl);
    if (result) {
      window.location.href = result.originalUrl;
    }
  };

  const addClickListenerToShortenedUrl = () => {
    const shortUrlLink = shortenedUrlDiv.querySelector(".url");
    shortUrlLink.addEventListener("click", handleShortUrlClick);
  };

  shortenForm.addEventListener("submit", handleShortenFormSubmit);
});
