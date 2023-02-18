from bs4 import BeautifulSoup
import requests
import psycopg2

conn = psycopg2.connect(
    host = 'localhost',
    dbname = 'Scraper',
    user = 'Marten',
    password = '9624',
    port = 5432
)

cur = conn.cursor()

headers = {'User-Agent':"Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:109.0) Gecko/20100101 Firefox/109.0"}

def demographics(thistconst):
    url = "https://www.imdb.com/title/" + thistconst + "/ratings/"
    response = requests.get(url, headers=headers, timeout=5)
    soup = BeautifulSoup(response.content, 'html5lib')

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

    print(rs)
    print(vs)

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

file = open('Trial.txt')
lines = file.read().splitlines()
file.close()

for line in lines:
    print(line)
    demographics(line)

cur.close()
conn.close()