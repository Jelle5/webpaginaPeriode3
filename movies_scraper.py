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
'http://kvquxzla:lf9uk8xavtyr@45.155.68.129:8133'
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
    try:
        response = requests.get(url, headers=headers, proxies=proxies, timeout=5)
        response.raise_for_status()
        return response.content
    except requests.exceptions.ReadTimeout as e:
        file = open('errors.txt', 'w')
        file.write(str(url))
    except requests.exceptions.RequestException as e:
        file = open('errors.txt', 'w')
        file.write(str(url))

def query(movie):

    #insert into movie
    sql_movie = """INSERT INTO movies (tconst, budget, gross_domestic, gross_worldwide, opening_weekend, certificate, origin, filming_country, aspect_ratio)
    VALUES (%s, %s,%s, %s,%s, %s,%s, %s,%s) ON CONFLICT DO NOTHING;"""

    cur.execute(sql_movie, (movie["tconst"],movie["budget"],movie["grossdomestic"],movie["grossworldwide"],movie["openingweekend"],movie["certificate"],movie["origin"],movie["filmingcountry"],movie["aspectratio"]))

    #insert sites
    for i in range(10):
        if movie["site" + str(i)] == None:
            break
        sql_site = """
            INSERT INTO site (name, link)
            SELECT %s, %s
            WHERE NOT EXISTS (
                SELECT 1 FROM site WHERE name = %s AND link = %s
            )
            RETURNING sconst;
            """
        cur.execute(sql_site, (
            movie["site" + str(i)], movie["link" + str(i)], movie["site" + str(i)],
            movie["link" + str(i)]))

        sconst = cur.fetchone()

        # Check if sconst has a value
        if sconst is None:
            # Site already exists, retrieve sconst value
            sql_get_sconst = """
                SELECT sconst FROM site WHERE name = %s AND link = %s;
            """
            cur.execute(sql_get_sconst, (movie["site" + str(i)], movie["link" + str(i)]))
            sconst = cur.fetchone()

        # Check if the site-tconst combination already exists in the sites table
        sql_check_sites = """
            SELECT 1 FROM sites WHERE tconst = %s AND sconst = %s;
        """
        cur.execute(sql_check_sites, (movie["tconst"], sconst[0]))
        if cur.fetchone():
            # Site-tconst combination already exists, skip this iteration
            continue

        # Insert into sites table
        sql_sites = ("""INSERT INTO sites (tconst, sconst) VALUES (%s, %s)""")
        cur.execute(sql_sites, (movie["tconst"], sconst[0]))

    #insert into languages
    for i in range(10):
        if movie["language" + str(i)] == None:
            break
        sql_language = """
            INSERT INTO language (name)
            SELECT %s
            WHERE NOT EXISTS (
                SELECT 1 FROM language WHERE name = %s
            )
            RETURNING lconst;
            """
        cur.execute(sql_language, (
            movie["language" + str(i)], movie["language" + str(i)]))

        lconst = cur.fetchone()

        # Check if lconst has a value
        if lconst is None:
            # Site already exists, retrieve lconst value
            sql_get_lconst = """
                SELECT lconst FROM language WHERE name = %s
            """
            cur.execute(sql_get_lconst, (movie["language" + str(i)],))
            lconst_value = cur.fetchone()
            if lconst_value is not None:
                lconst = lconst_value

        # Check if the language-tconst combination already exists in the languages table
        sql_check_languages = """
            SELECT 1 FROM languages WHERE tconst = %s AND lconst = %s;
        """
        cur.execute(sql_check_languages, (movie["tconst"], lconst[0]))
        if cur.fetchone():
            # Site-tconst combination already exists, skip this iteration
            continue

        # Insert into languages table
        sql_languages = ("""INSERT INTO languages (tconst, lconst) VALUES (%s, %s)""")
        cur.execute(sql_languages, (movie["tconst"], lconst[0]))

    #insert into colors
    for i in range(10):
        if movie["color" + str(i)] == None:
            break
        sql_color = """
            INSERT INTO color (name)
            SELECT %s
            WHERE NOT EXISTS (
                SELECT 1 FROM color WHERE name = %s
            )
            RETURNING cconst;
            """
        cur.execute(sql_color, (
            movie["color" + str(i)], movie["color" + str(i)]))

        cconst = cur.fetchone()

        # Check if cconst has a value
        if cconst is None:
            # Site already exists, retrieve cconst value
            sql_get_cconst = """
                SELECT cconst FROM color WHERE name = %s
            """
            cur.execute(sql_get_cconst, (movie["color" + str(i)],))
            cconst_value = cur.fetchone()
            if cconst_value is not None:
                cconst = cconst_value

        # Check if the color-tconst combination already exists in the colors table
        sql_check_colors = """
            SELECT 1 FROM colors WHERE tconst = %s AND cconst = %s;
        """
        cur.execute(sql_check_colors, (movie["tconst"], cconst[0]))
        if cur.fetchone():
            # Site-tconst combination already exists, skip this iteration
            continue

        # Insert into colors table
        sql_colors = ("""INSERT INTO colors (tconst, cconst) VALUES (%s, %s)""")
        cur.execute(sql_colors, (movie["tconst"], cconst[0]))

    #insert into soundmixes
    for i in range(10):
        if movie["soundmix" + str(i)] == None:
            break
        sql_soundmix = """
            INSERT INTO soundmix (name)
            SELECT %s
            WHERE NOT EXISTS (
                SELECT 1 FROM soundmix WHERE name = %s
            )
            RETURNING soconst;
            """
        cur.execute(sql_soundmix, (
            movie["soundmix" + str(i)], movie["soundmix" + str(i)]))

        soconst = cur.fetchone()

        # Check if soconst has a value
        if soconst is None:
            # Site already exists, retrieve soconst value
            sql_get_soconst = """
                SELECT soconst FROM soundmix WHERE name = %s
            """
            cur.execute(sql_get_soconst, (movie["soundmix" + str(i)],))
            soconst_value = cur.fetchone()
            if soconst_value is not None:
                soconst = soconst_value

        # Check if the soundmix-tconst combination already exists in the soundmixes table
        sql_check_soundmixes = """
            SELECT 1 FROM soundmixes WHERE tconst = %s AND soconst = %s;
        """
        cur.execute(sql_check_soundmixes, (movie["tconst"], soconst[0]))
        if cur.fetchone():
            # Site-tconst combination already exists, skip this iteration
            continue

        # Insert into soundmixes table
        sql_soundmixes = ("""INSERT INTO soundmixes (tconst, soconst) VALUES (%s, %s)""")
        cur.execute(sql_soundmixes, (movie["tconst"], soconst[0]))

    #insert into companies
    for i in range(10):
        if movie["company" + str(i)] == None:
            break
        sql_company = """
            INSERT INTO company (name)
            SELECT %s
            WHERE NOT EXISTS (
                SELECT 1 FROM company WHERE name = %s
            )
            RETURNING coconst;
            """
        cur.execute(sql_company, (
            movie["company" + str(i)], movie["company" + str(i)]))

        coconst = cur.fetchone()

        # Check if coconst has a value
        if coconst is None:
            # Site already exists, retrieve coconst value
            sql_get_coconst = """
                SELECT coconst FROM company WHERE name = %s
            """
            cur.execute(sql_get_coconst, (movie["company" + str(i)],))
            coconst_value = cur.fetchone()
            if coconst_value is not None:
                coconst = coconst_value

        # Check if the company-tconst combination already exists in the companies table
        sql_check_companies = """
            SELECT 1 FROM companies WHERE tconst = %s AND coconst = %s;
        """
        cur.execute(sql_check_companies, (movie["tconst"], coconst[0]))
        if cur.fetchone():
            # Site-tconst combination already exists, skip this iteration
            continue

        # Insert into companies table
        sql_companies = ("""INSERT INTO companies (tconst, coconst) VALUES (%s, %s)""")
        cur.execute(sql_companies, (movie["tconst"], coconst[0]))
    conn.commit()

def movies(soup, thistconst):

    movie = {}

    movie["tconst"] = thistconst

    budget_pattern = r"\$?([\d,]+)(?:\s*\((?:estimated|unconfirmed)\))?"

    budget = soup.find('li', attrs={'data-testid': 'title-boxoffice-budget'})
    if budget is not None:
        budget_text = budget.find('label').text.strip()
        match = re.search(budget_pattern, budget_text)
        if match:
            movie["budget"] = int(match.group(1).replace(',', ''))
        else:
            movie["budget"] = None
    else:
        movie["budget"] = None

    grossdomestic = soup.find('li', attrs={'data-testid': 'title-boxoffice-grossdomestic'})
    if grossdomestic is not None:
        domestic_text = grossdomestic.find('label').text.strip()
        match = re.search(budget_pattern, domestic_text)
        if match:
            movie["grossdomestic"] = int(match.group(1).replace(',', ''))
        else:
            movie["grossdomestic"] = None
    else:
        grossdomestic = None
        movie["grossdomestic"] = grossdomestic

    grossworldwide = soup.find('li', attrs={'data-testid': 'title-boxoffice-cumulativeworldwidegross'})
    if grossworldwide is not None:
        movie["grossworldwide"] = grossworldwide.find('label').text[1:].replace(',','').strip()
    else:
        grossworldwide = None
        movie["grossworldwide"] = grossworldwide

    openingweekend = soup.find('li', attrs={'data-testid': 'title-boxoffice-openingweekenddomestic'})
    if openingweekend is not None:
        movie["openingweekend"] = openingweekend.find('label').text[1:].replace(',','').strip()
    else:
        openingweekend = None
        movie["openingweekend"] = openingweekend

    certificate = soup.find('li', attrs={'data-testid':'storyline-certificate'})
    if certificate is not None:
        movie["certificate"] = certificate.find('label').text.strip()
    else:
        certificate = None
        movie["certificate"] = certificate

    origin = soup.find('li', attrs={'data-testid': 'title-details-origin'})
    if origin is not None:
        movie["origin"] = origin.find('a').text.strip()
    else:
        origin = None
        movie["origin"] = origin

    sites = soup.find('li', attrs={'data-testid':'details-officialsites'})
    if sites is not None:
        sites = sites.findAll('li')
        for i in range(10):
            if i < len(sites):
                movie["site" + str(i)] = sites[i].find('a').text.strip()
                movie["link" + str(i)] = sites[i].find('a').get('href')
            else:
                movie["site" + str(i)] = None
                movie["link" + str(i)] = None
    else:
        movie["site0"] = None
        movie["site1"] = None
        movie["site2"] = None
        movie["link0"] = None
        movie["link1"] = None
        movie["link2"] = None

    languages = soup.find('li', attrs={'data-testid': 'title-details-languages'})
    if languages is not None:
        languages = languages.findAll('li')
        for i in range(10):
            if i < len(languages):
                movie["language" + str(i)] = languages[i].find('a').text.strip()
            else:
                movie["language" + str(i)] = None
    else:
        movie["language0"] = None
        movie["language1"] = None
        movie["language2"] = None

    location = soup.find('li', attrs={'data-testid': 'title-details-filminglocations'})
    if location is not None:
        location = location.find('div').find('a').text.strip()
        country_regex = r'([^,]+$)'
        country_match = re.search(country_regex, location)
        if country_match:
            country = country_match.group(1)
            movie["filmingcountry"] = country
    else:
        country = None
        movie["filmingcountry"] = country

    companies = soup.find('li', attrs={'data-testid': 'title-details-companies'})
    if companies is not None:
        companies = companies.findAll('li')
        for i in range(10):
            if i < len(companies):
                movie["company" + str(i)] = companies[i].find('a').text.strip()
            else:
                movie["company" + str(i)] = None
    else:
        movie["company0"] = None
        movie["company1"] = None
        movie["company2"] = None

    colors = soup.find('li', attrs={'data-testid': 'title-techspec_color'})
    if colors is not None:
        colors = colors.findAll('li')
        for i in range(10):
            if i < len(colors):
                movie["color" + str(i)] = colors[i].find('a').text.strip()
            else:
                movie["color" + str(i)] = None
    else:
        movie["color0"] = None
        movie["color1"] = None
        movie["color2"] = None

    soundmixes = soup.find('li', attrs={'data-testid': 'title-techspec_soundmix'})
    if soundmixes is not None:
        soundmixes = soundmixes.findAll('li')
        for i in range(10):
            if i < len(soundmixes):
                movie["soundmix" + str(i)] = soundmixes[i].find('a').text.strip()
            else:
                movie["soundmix" + str(i)] = None
    else:
        movie["soundmix0"] = None
        movie["soundmix1"] = None
        movie["soundmix2"] = None

    aspect_pattern = r'\d+\.\d+:\d+'  # pattern for aspect ratio (e.g. 1.85:1)

    aspect = soup.find('li', attrs={'data-testid': 'title-techspec_aspectratio'})
    if aspect is not None:
        aspect_text = aspect.find('label').text.replace(' ', '').strip()
        aspect_ratio = re.search(aspect_pattern, aspect_text)
        if aspect_ratio:
            movie["aspectratio"] = aspect_ratio.group()
        else:
            movie["aspectratio"] = None
    else:
        movie["aspectratio"] = None

    query(movie)

file = open('allmoviesandseries.txt')
lines = file.read().splitlines()
file.close()

for line in lines:
    cur.execute("SELECT tconst FROM title WHERE tconst=%s", (line,))
    if cur.fetchone() is not None:
        # line already exists in database, skip to next line
        continue

    url = "https://www.imdb.com/title/" + line
    proxy = next(proxy_pool)
    proxies = {'http': proxy}
    try:
        response = requests.get(url, headers=headers, proxies=proxies, timeout=5)
        response.raise_for_status()
        soup = BeautifulSoup(response.content, 'html5lib')
        movies(soup, line)
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