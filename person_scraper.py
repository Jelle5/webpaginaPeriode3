import itertools

from bs4 import BeautifulSoup
import requests
import itertools
import psycopg2

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

def demographics(thistconst):
    url = "https://www.imdb.com/title/" + thistconst + "/ratings/"
    response = make_request(url)
    soup = BeautifulSoup(response, 'html5lib')

    table = soup.findAll('table')[1]

    print(table.prettify())

    conn.commit()

file = open('Trial.txt')
lines = file.read().splitlines()
file.close()

for line in lines:
    print(line)
    demographics(line)

cur.close()
conn.close()