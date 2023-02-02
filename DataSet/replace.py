with open('movie urls.txt', 'r') as file :
	filedata = file.read();

filedata = filedata.replace('tt', 'https://m.imdb.com/title/tt')
#filedata = filedata+s

with open('movie urls.txt', 'w') as file :
	file.write(filedata)
