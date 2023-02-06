import csv

from bs4 import BeautifulSoup
import requests

headers = {'User-Agent':"Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:109.0) Gecko/20100101 Firefox/109.0"}

movies = []

def demographics(move, tconst):
    url = "https://www.imdb.com/title/" + tconst + "/ratings/"

    response = requests.get(url, headers=headers, timeout=5)
    soup = BeautifulSoup(response.content, 'html5lib')

    table = soup.find('table')
    if table is not None:
        rows = table.findAll('tr')
        movie_rating = {}

        for row in rows:
            movie_rating["tconst"] = tconst

            cols = row.find('td')
            for col in cols:


def ratings(movie, tconst):
    url = "https://www.imdb.com/title/" + tconst + "/ratings/"

    response = requests.get(url, headers=headers, timeout=5)
    soup = BeautifulSoup(response.content, 'html5lib')

    table = soup.find('table')
    if table is not None:
        rows = table.findAll('tr')
        movie_rating = {}

        for row in rows:
            movie_rating["tconst"] = tconst

            rating = row.find('div', attrs={'class':'rightAligned'})
            if rating is not None:
                movie_rating["rating"] = rating.text.strip()

            percentage = row.find('div', attrs={'class':'topAligned'})
            if percentage is not None:
                movie_rating["percentage"] = percentage.text.strip()

            votes = row.find('div', attrs={'class':'leftAligned'})
            if votes is not None:
                movie_rating["votes"] = votes.text.strip()

            print(movie_rating)

def extract(tconst, soup):
    movie = {}

    movie["tconst"] = tconst

    #Budget
    Budget = soup.find('li', attrs={'data-testid':'title-boxoffice-budget'})
    if Budget is not None:
        movie["Budget"] = Budget.find('label').text

    #GrossDomestic
    GrossDomestic = soup.find('li', attrs={'data-testid': 'title-boxoffice-grossdomestic'})
    if GrossDomestic is not None:
        movie["GrossDomestic"] = GrossDomestic.find('label').text

    #GrossWorldwide
    GrossWorldwide = soup.find('li', attrs={'data-testid': 'title-boxoffice-cumulativeworldwidegross'})
    if GrossWorldwide is not None:
        movie["GrossWorldwide"] = GrossWorldwide.find('label').text

    #OpeningWeekend
    OpeningWeekend = soup.find('li', attrs={'data-testid': 'title-boxoffice-openingweekenddomestic'})
    if OpeningWeekend is not None:
        movie["OpeningWeekend"] = OpeningWeekend.find('label').text

    #Color
    Color = soup.find('li', attrs={'data-testid':'title-techspec_color'})

    if Color is not None:
        if len(Color.findAll('li')) > 1:
            Colors = Color.findAll('li')
            i = 1
            for color in Colors:
                movie["Color" + str(i)] = color.find('a').text
                i += 1

        else:
            movie["Color"] = Color.find('a').text

    #SoundMix
    SoundMix = soup.find('li', attrs={'data-testid':'title-techspec_soundmix'})

    if SoundMix is not None:
        if len(SoundMix.findAll('li')) > 1:
            Mixes = SoundMix.findAll('li')
            i = 1
            for mix in Mixes:
                movie["SoundMix" + str(i)] = mix.find('a').text
                i += 1

        else:
            movie["SoundMix"] = SoundMix.find('a').text

    print(movie)

    ratings(movie, tconst)

def scrape(tconst, url):
    r = requests.get(url, headers=headers, timeout=5)
    soup = BeautifulSoup(r.content, 'html5lib')
    extract(tconst, soup)

file = open('Trial.txt')
lines = file.read().splitlines()
file.close()

for line in lines:
    scrape(line, 'https://www.imdb.com/title/' + line)
