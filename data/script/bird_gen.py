from faker import Faker
from datetime import datetime
import random
import configparser
import uuid

# Load configuration
config = configparser.ConfigParser()
config.read('config.ini')
owner_id = config['Birds']['OWNER']

# Create Faker instance
fake = Faker()

# Open output file for writing
with open('../sql/birds.sql', 'w') as file:

    # Generate 1000 bird records
    for i in range(30):

        # Generate fake bird data
        name = fake.name()
        age = random.randint(1, 20)
        weight = round(random.uniform(0.1, 5.0), 2)
        elo = random.randint(1, 100)
        status = fake.random_element(elements=('Available', 'Sold', 'Reserved', 'Unavailable'))
        created_datetime = fake.date_time_between(start_date='-1y', end_date='now').strftime('%Y-%m-%d %H:%M:%S')
        description = fake.sentence(nb_words=10)
        color = fake.color_name()
        bird_type_id = '91e851d2-4997-4f71-59c9-08db08684efa'
        is_deleted = random.choice([True]*10 + [False]*90)
        # Write SQL insert statement to output file
        file.write("INSERT INTO Bird (Id, BirdName, BirdAge, BirdWeight, BirdElo, BirdStatus, BirdCreatedDatetime, BirdDescription, BirdColor, BirdTypeId, OwnerId,IsDeleted) VALUES ('{}', '{}', {}, {}, {}, '{}', '{}', '{}', '{}', '{}', '{}','{}');\n".format(
            uuid.uuid4(), name, age, weight, elo, status, created_datetime, description, color, bird_type_id, owner_id,is_deleted))
