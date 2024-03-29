from faker import Faker
import random
import uuid
from enum import Enum
# Create Faker instance
fake = Faker()

# Define role enum
class Role(Enum):
    User = 1
    Admin = 2

import hashlib
import base64

# generate a random password using fake library
random_password = '12345'

# encode password to bytes and hash
encoded_password = str(random_password).encode('utf-8')
hashed_password = hashlib.sha256(encoded_password).digest()

# convert hashed password to base64 string format
password_base64 = base64.b64encode(hashed_password).decode('utf-8')
# Open output file for writing
print(password_base64)
with open('data/sql/account.sql', 'w+') as file:

    # Generate 1000 user records
    for i in range(1000):
        
        # Generate fake user data
        first_name = fake.first_name()
        last_name = fake.last_name()
        username = (first_name + last_name + str(random.randint(1, 999))).lower()
        email = fake.email()
        password = fake.password()
        role = random.choice([Role.User, Role.Admin])
        create_date_time = fake.date_time_between(start_date='-1y', end_date='now').strftime('%Y-%m-%d %H:%M:%S')
        description = fake.sentence(nb_words=10)
        is_deleted = random.choice([True]*10 + [False]*90)
        id = str(uuid.uuid4())

        # Write SQL insert statement to output file
        file.write("INSERT INTO Account (Id, Password, Email, CreateDateTime, FirstName, LastName, Role, Username, Description, IsDeleted) VALUES ('{}', '{}', '{}', '{}', '{}', '{}', '{}', '{}', '{}', '{}');\n".format(
            id, password_base64, email, create_date_time, first_name, last_name, role.value, username, description, int(is_deleted)))  # use 0 or 1 for boolean fields in SQL

