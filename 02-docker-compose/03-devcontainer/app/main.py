from functools import lru_cache
from typing import AsyncGenerator

from fastapi import Depends, FastAPI
from pydantic import BaseModel
from pydantic_settings import BaseSettings, SettingsConfigDict
from sqlalchemy.ext.asyncio import create_async_engine
from sqlalchemy.sql import text
from sqlmodel.ext.asyncio.session import AsyncSession


class Settings(BaseSettings):
    app_name: str = "Super Simple App"
    debug: bool = False
    db_url: str

    model_config = SettingsConfigDict(env_prefix="APP_")


# Singleton pattern for settings
@lru_cache
def get_settings() -> Settings:
    return Settings()


#  Create DB Setting
DATABASE_URL = get_settings().db_url

engine = create_async_engine(DATABASE_URL)


async def get_db_session() -> AsyncGenerator[AsyncSession, None]:
    async with AsyncSession(engine) as session:
        try:
            yield session
        finally:
            await session.close()


class HealthCheckStatus(BaseModel):
    status: str


app = FastAPI()


@app.get("/api/_healthz/liveness", tags=["healthz"], response_model=HealthCheckStatus)
def get_liveness_status():
    status = "ok" if not get_settings().db_url else "error"
    return {"status": status}


@app.get("/api/_healthz/readiness", tags=["healthz"], response_model=HealthCheckStatus)
async def get_readiness_status(db: AsyncSession = Depends(get_db_session)):
    result = await db.exec(text("SELECT 1;"))
    status = "ok" if result.first() == (1,) else "error"
    return {"status": status}
