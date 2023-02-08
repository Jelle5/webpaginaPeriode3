import csv

from bs4 import BeautifulSoup
import requests
import csv

headers = {'User-Agent':"Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:109.0) Gecko/20100101 Firefox/109.0"}

movies = []
demographs = []
movies_ratings = []

def demographics(movie, tconst):
    url = "https://www.imdb.com/title/" + tconst + "/ratings/"

    response = requests.get(url, headers=headers, timeout=5)
    soup = BeautifulSoup(response.content, 'html5lib')

    if soup.find('div', attrs={'class':'sectionHeading'}) == "IMDb users":

            table = soup.findAll('table')[1]

            rows = table.findAll('tr')[1:]
            demograph = {}

            i = 0
            for row in rows:
                demograph["tconst"] = tconst
                cols = row.findAll('td')

                if i == 0:
                    demograph["All_All"] = cols[1].text.strip().replace("\n            \n                \n                    ","/")
                    demograph["All_under_18"] = cols[2].text.strip().replace("\n            \n                \n                    ","/")
                    demograph["All_18_29"] = cols[3].text.strip().replace("\n            \n                \n                    ","/")
                    demograph["All_30_44"] = cols[4].text.strip().replace("\n            \n                \n                    ","/")
                    demograph["All_45_plus"] = cols[5].text.strip().replace("\n            \n                \n                    ","/")

                if i == 1:
                    demograph["Male_All"] = cols[1].text.strip().replace("\n            \n                \n                    ","/")
                    demograph["Male_under_18"] = cols[2].text.strip().replace("\n            \n                \n                    ","/")
                    demograph["Male_18_29"] = cols[3].text.strip().replace("\n            \n                \n                    ","/")
                    demograph["Male_30_44"] = cols[4].text.strip().replace("\n            \n                \n                    ","/")
                    demograph["Male_45_plus"] = cols[5].text.strip().replace("\n            \n                \n                    ","/")

                if i == 2:
                    demograph["Female_All"] = cols[1].text.strip().replace("\n            \n                \n                    ","/")
                    demograph["Female_under_18"] = cols[2].text.strip().replace("\n            \n                \n                    ","/")
                    demograph["Female_18_29"] = cols[3].text.strip().replace("\n            \n                \n                    ","/")
                    demograph["Female_30_44"] = cols[4].text.strip().replace("\n            \n                \n                    ","/")
                    demograph["Female_45_plus"] = cols[5].text.strip().replace("\n            \n                \n                    ","/")

                i = i + 1

            demographs.append(demograph)


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

        movies_ratings.append(movie_rating)

    demographics(movie,tconst)

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

    movies.append(movie)

    ratings(movie, tconst)

def scrape(tconst, url):
    r = requests.get(url, headers=headers, timeout=5)
    soup = BeautifulSoup(r.content, 'html5lib')
    extract(tconst, soup)

file = open('Trial.txt')
lines = file.read().splitlines()
file.close()

for line in lines:
    print(line)
    scrape(line, 'https://www.imdb.com/title/' + line)


filename = 'movies.csv'
with open(filename, 'w', newline='') as f:
    w = csv.DictWriter(f,['tconst','budget','grossdomestic','grossworldwide','openingweekend','color0','color1','color2','soundmix0','soundmix1','soundmix2'])
    w.writeheader()
    for movie in movies:
        w.writerow(movie)

filename = 'ratings.csv'
with open(filename, 'w', newline='') as f:
    w = csv.DictWriter(f,['tconst','rating','percentage','votes'])
    w.writeheader()
    for rating in ratings:
        w.writerow(rating)

filename = 'demograph.csv'
with open(filename, 'w', newline='') as f:
    w = csv.DictWriter(f,['tconst','All_All','All_under_18','All_under_18','Male_All','Male_under_18','Male_18_29','Male_30_44','Male_45_plus','Female_All','Female_under_18','Female_18_29','Female_30_44','Female_45_plus'])
    w.writeheader()
    for demograph in demographs:
        w.writerow(demograph)