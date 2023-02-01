with open('imdb-urls.txt', 'r') as file :
	filedata = file.read();


filedata = filedata.replace('tt', 'https://m.imdb.com/title/tt')

with open('imdb-urls.txt', 'w') as file :
	file.write(filedata)