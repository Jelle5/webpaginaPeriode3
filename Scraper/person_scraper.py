import itertools

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
    dbname = 'imdb',
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

def person(osup, thisnconst):
    person = {}

    person["tconst"] = thisnconst

    if soup.find('li', attrs={'data-testid':'award_information'}) is not None:
        awards = soup.find('li', attrs={'data-testid':'award_information'})
        person["primaryAward"] = awards.find('a').text.strip()
        print(person["primaryAward"])

        subAward = awards.find('div').find('label').text.strip()

        wins_pattern = r"\d+(?=\s+wins)"
        wins_match = re.search(wins_pattern, subAward)
        wins = ''
        if wins_match:
            wins = wins_match.group()

        nominations_pattern = r"\d+(?=\s+nominations?)"
        nominations_match = re.search(nominations_pattern, subAward)
        nominations = ''
        if nominations_match:
            nominations = nominations_match.group()

        person["nrWins"] = wins
        person["nrNominations"] = nominations
    else:
        person["primaryAward"] = None
        person["nrWins"] = None
        person["nrNominations"] = None

    sites = soup.find('li', attrs={'data-testid': 'details-officialsites'})
    if sites is not None:
        sites = sites.findAll('li')
        for i in range(10):
            if i < len(sites):
                person["site" + str(i)] = sites[i].find('a').text.strip()
                person["link" + str(i)] = sites[i].find('a').get('href')
            else:
                person["site" + str(i)] = None
                person["link" + str(i)] = None
    else:
        person["site0"] = None
        person["site1"] = None
        person["site2"] = None
        person["link0"] = None
        person["link1"] = None
        person["link2"] = None

    if soup.find('li', attrs={'data-testid':'nm_pd_he'}) is not None:
        person["height"] = soup.find('li', attrs={'data-testid':'nm_pd_he'}).find('label').text.replace('m','').replace('.','').strip()
    else:
        person["height"] = None

    print(person)

    conn.commit()

file = open('Persons.txt')
lines = file.read().splitlines()
file.close()

for line in lines:
    cur.execute("SELECT nconst FROM individual WHERE nconst=%s", (line,))
    '''
        if cur.fetchone() is not None:
        # line already exists in database, skip to next line
        print(cur.fetchone())
        continue
    '''

    print(line)

    url = "https://www.imdb.com/name/" + line
    proxy = next(proxy_pool)
    proxies = {'http': proxy}
    try:
        response = requests.get(url, headers=headers, proxies=proxies, timeout=5)
        response.raise_for_status()
        soup = BeautifulSoup(response.content, 'html5lib')
        person(soup, line)
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



cur.close()
conn.close()