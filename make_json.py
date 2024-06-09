import json
import random
from faker import Faker

fake = Faker()

def generate_random_activity(category_types):
    start = round(4 * random.uniform(8, 18)) / 4
    end = round(4 * min(random.uniform(start, start + 4), 22)) / 4
    remark = fake.sentence()
    topic = fake.word()
    location = fake.city()
    category = random.sample(category_types, 1)[0]
    instructor = fake.name()
    description = fake.text()

    return {
        "Remark": remark,
        "Start": start,
        "End": end,
        "Topic": topic,
        "Location": location,
        "Category": category,
        "Instructor": instructor,
        "Description": description
    }

def generate_random_day(category_types):
    weekday = fake.day_of_week()
    daily_supervisor = fake.name()
    activities = [generate_random_activity(category_types) for _ in range(5)]

    return {
        "Weekday": weekday,
        "DailySupervisor": daily_supervisor,
        "Activities": activities
    }

def generate_random_retreat_supervisors():
    name = fake.name()
    file_url = fake.url()

    return {
        "Name": name,
        "FileUrl": file_url
    }

def generate_random_category_color():
    category_name = fake.word()
    color_code = fake.hex_color()

    return {
        "CategoryName": category_name,
        "ColorCode": color_code
    }

def generate_random_meme():
    file_type = fake.file_extension()
    file_url = fake.url()
    description = fake.sentence()

    return {
        "FileType": file_type,
        "FileUrl": file_url,
        "Description": description
    }

def generate_random_schedule():
    retreat_topic = fake.sentence()
    urgent = fake.boolean(chance_of_getting_true=50)
    memes = [generate_random_meme() for _ in range(2)]
    color_scheme = [generate_random_category_color() for _ in range(3)]
    color_scheme.append({"CategoryName": "FreeTime", "ColorCode": fake.hex_color()})
    category_types = [item['CategoryName'] for item in color_scheme]
    retreat_supervisors = [generate_random_retreat_supervisors() for _ in range(3)]
    days = [generate_random_day(category_types) for _ in range(5)]

    return {
        "RetreatTopic": retreat_topic,
        "Urgent": urgent,
        "Memes": memes,
        "ColorScheme": color_scheme,
        "RetreatSupervisors": retreat_supervisors,
        "Days": days
    }

def main():
    random_schedule = generate_random_schedule()
    json_data = json.dumps(random_schedule, indent=2)
    return json_data

if __name__ == "__main__":
    json_data = main()

    with open("out.json", "w+") as f:
        f.write(json_data)


