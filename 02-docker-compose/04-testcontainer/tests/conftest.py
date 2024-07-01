import sys
import uuid
import pytest
import os
from os.path import dirname as d
from os.path import abspath
from testcontainers.postgres import PostgresContainer

root_dir = d(d(abspath(__file__)))
sys.path.append(root_dir)


@pytest.fixture(autouse=True)
def db(request):
    postgres = PostgresContainer(
        "postgres:alpine",
        dbname=f"test_{uuid.uuid4()}",
        driver='postgresql+asyncpg')

    postgres.start()

    def remove_container():
        postgres.stop()

    request.addfinalizer(remove_container)

    os.environ["APP_DB_URL"] = postgres.get_connection_url()
