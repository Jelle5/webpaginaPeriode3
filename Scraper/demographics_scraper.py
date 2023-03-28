from bs4 import BeautifulSoup
import requests
import itertools
import psycopg2
import re

proxies = [
'http://kvquxzla:lf9uk8xavtyr@185.199.229.156:7492'
'http://kvquxzla:lf9uk8xavtyr@185.199.228.220:7300'
'http://kvquxzla:lf9uk8xavtyr@185.199.231.45:8382'
'http://kvquxzla:lf9uk8xavtyr@188.74.210.207:6286'
'http://kvquxzla:lf9uk8xavtyr@188.74.183.10:8279'
'http://kvquxzla:lf9uk8xavtyr@188.74.210.21:6100'
'http://:lf9uk8xavtyr@45.155.68.129:8133'
'http://kvquxzla:lf9uk8xavtyr@154.95.36.199:6893'
'http://kvquxzla:lf9uk8xavtyr@45.94.47.66:8110'
'http://kvquxzla:lf9uk8xavtyr@144.168.217.88:8780'
]

proxy_pool = itertools.cycle(proxies)

conn = psycopg2.connect(
    host = 'localhost',
    dbname = 'Scraper',
    user = 'Marten',
    password = '9624',
    port = 5432
)

cur = conn.cursor()

headers = {'User-Agent':"Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:109.0) Gecko/20100101 Firefox/109.0"}

def make_request(url):
    proxy = next(proxy_pool)
    proxies = {'http': proxy}
    response = requests.get(url, headers=headers, proxies=proxies, timeout=5)
    return response.content

def demographics(soup, thistconst):
    url = "https://www.imdb.com/title/" + thistconst + "/ratings/"
    table = soup.find('table')
    rows = table.findAll('tr')

    rating = []

    rating.append(thistconst)
    text = soup.find('div', attrs={'class':'allText'}).find('div', attrs={'class':'allText'}).text.strip()

    pattern = r"Arithmetic mean = (\d+\.\d+)\s+Median = (\d+)"
    match = re.search(pattern, text)

    rating.append(float(match.group(1)))
    rating.append(int(match.group(2)))



    for row in rows[1:]:
        cells = row.findAll('td')
        rating.append(cells[1].text.replace('%','').strip())
        rating.append(cells[2].text.replace(',','').strip())

    insert_query = """INSERT INTO ratings (
    tconst, mean, median,
    "10_percentage", "10_numvotes", 
    "9_percentage", "9_numvotes",
    "8_percentage", "8_numvotes",
    "7_percentage", "7_numvotes",
    "6_percentage", "6_numvotes",
    "5_percentage", "5_numvotes",
    "4_percentage", "4_numvotes",
    "3_percentage", "3_numvotes",
    "2_percentage", "2_numvotes",
    "1_percentage", "1_numvotes"
) 
VALUES (%s, %s, %s, 
    %s, %s,
    %s, %s,
    %s, %s,
    %s, %s,
    %s, %s,
    %s, %s,
    %s, %s,
    %s, %s,
    %s, %s,
    %s, %s
);
"""

    cur.execute(insert_query, (rating[0], rating[1], rating[2], rating[3], rating[4], rating[5], rating[6], rating[7], rating[8], rating[9], rating[10], rating[11], rating[12], rating[13], rating[14], rating[15], rating[16], rating[17], rating[18], rating[19], rating[20], rating[21], rating[22]))

    table = soup.findAll('table')[1]

    rs = []
    vs = []

    rows = table.findAll('tr')[1:]

    for row in rows:
        cols = row.findAll('td')[1:]

        for col in cols:
            cells = col.findAll('div')

            if col.find('div', attrs={'class':'bigcell'}) is None:
                r = None
                rs.append(r)

            else:
                if col.find('div', attrs={'class':'bigcell'}).text.strip() == '-':
                    r = None
                    rs.append(r)
                else:
                    rs.append(col.find('div', attrs={'class':'bigcell'}).text.strip())


            if col.find('div', attrs={'class': 'smallcell'}) is None:
                v = None
                vs.append(v)
            else:
                vs.append(col.find('div', attrs={'class': 'smallcell'}).find('a').text.strip().replace(',',''))

    for r in rs:
        if r is not None:
            r = float(r)

    for v in vs:
        if v is not None:
            v = int(v)

    query = """
                INSERT INTO demographics 
                (tconst, all_all_ages_rating, all_under_18_rating, all_18_29_rating, all_30_44_rating, all_45_plus_rating, 
                male_all_ages_rating, male_under_18_rating, male_18_29_rating, male_30_44_rating, male_45_plus_rating, 
                female_all_ages_rating, female_under_18_rating, female_18_29_rating, female_30_44_rating, female_45_plus_rating,
                all_all_ages_numvotes, all_under_18_numvotes, all_18_29_numvotes, all_30_44_numvotes, all_45_plus_numvotes, 
                male_all_ages_numvotes, male_under_18_numvotes, male_18_29_numvotes, male_30_44_numvotes, male_45_plus_numvotes, 
                female_all_ages_numvotes, female_under_18_numvotes, female_18_29_numvotes, female_30_44_numvotes, female_45_plus_numvotes)
                VALUES 
                (%s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s) ON CONFLICT DO NOTHING;"""

    cur.execute(query, (thistconst, rs[0], rs[1], rs[2], rs[3], rs[4], rs[5], rs[6], rs[7], rs[8], rs[9], rs[10], rs[11], rs[12], rs[13],
    rs[14], vs[0], vs[1], vs[2], vs[3], vs[4], vs[5], vs[6], vs[7], vs[8], vs[9], vs[10], vs[11], vs[12], vs[13],
    vs[14]))
    conn.commit()

file = open('allmoviesandseries.txt')
lines = file.read().splitlines()
file.close()

for line in lines:
    cur.execute("SELECT tconst FROM demographics WHERE tconst=%s", (line,))
    if cur.fetchone() is not None:
        # line already exists in database, skip to next line
        print(cur.fetchone())
        continue

    url = "https://www.imdb.com/title/" + line + "/ratings/"
    proxy = next(proxy_pool)
    proxies = {'http': proxy}
    try:
        response = requests.get(url, headers=headers, proxies=proxies, timeout=5)
        response.raise_for_status()
        soup = BeautifulSoup(response.content, 'html5lib')
        demographics(soup, line)
    except requests.exceptions.ReadTimeout as e:
        file = open('errors.txt', 'a')
        file.write('\n')
        file.write(line)
        file.close()
    except requests.exceptions.RequestException as e:
        file = open('othererrors.txt', 'a')
        file.write('\n')
        file.write(line)
        file.close()

    print(line)

cur.close()
conn.close()