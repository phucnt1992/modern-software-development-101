FROM tiangolo/uvicorn-gunicorn-fastapi:python3.11-slim

# Install deps
RUN apt update \
  && apt full-upgrade -y \
  && apt install -y libpq-dev gcc curl \
  && apt autoremove -y \
  && apt autoclean \
  && rm -rf /var/lib/apt/lists/*

WORKDIR /app

COPY ./requirements.txt /app/requirements.txt

RUN pip install --no-cache-dir --upgrade -r /app/requirements.txt

COPY ./app /app
