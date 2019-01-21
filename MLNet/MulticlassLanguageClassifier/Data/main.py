import wikipedia
import re
from sklearn.model_selection import train_test_split
from random import shuffle
import nltk.data
import numpy as np
from nltk.tokenize import TreebankWordTokenizer
# old r'\[[^)]*\]|[=0-9%"“«»‘’:.,\')();]'
# To remove special characters and endlines.
reg = r'\[[^)]*\]|[=\n\r"]'
# reg = r'\/^[^ ][\w\W ]*[^ ]/'
langs = {
    "en": nltk.data.load('tokenizers/punkt/english.pickle'),
    "tr": nltk.data.load('tokenizers/punkt/turkish.pickle'),
    "es": nltk.data.load('tokenizers/punkt/spanish.pickle')
}

# Wikipedia page titles
titles = []

titles.append(("Net neutrality", "en"))
titles.append(("Abraham Lincoln", "en"))
titles.append(("United States", "en"))
titles.append(("Jim Carrey", "en"))
titles.append(("Turkey", "en"))
titles.append(("Programming language", "en"))
titles.append(("World War I", "en"))

titles.append(("Mustafa Kemal Atatürk", "tr"))
titles.append(("Türkiye", "tr"))
titles.append(("Türkiye'de mimarlık", "tr"))
titles.append(("Kemal Sunal", "tr"))
titles.append(("İlber Ortaylı", "tr"))
titles.append(("I. Dünya Savaşı", "tr"))
titles.append(("Programlama dili", "tr"))

titles.append(("España", "es"))
titles.append(("Elecciones al Parlamento de Cataluña de 2017", "es"))
titles.append(("Pablo Picasso", "es"))
titles.append(("Antonio Banderas", "es"))
titles.append(("Turquía", "es"))
titles.append(("Derechos de la mujer", "es"))
titles.append(("Primera Guerra Mundial", "es"))
titles.append(("Lenguaje de programación", "es"))

# Get content of the given title and language
def get_content(title, lang):
    print ("Getting "+ title)
    # Set page language of wiki api
    wikipedia.set_lang(lang)
    # Fetch the page
    page = wikipedia.page(title)
    # content = page.content
    content = page.content
    return parse_content(content, lang)

# Parse the given content
def parse_content(content, lang):
    # Get the nltk tokenizer
    tokenizer = langs[lang]
    # Tokenize the given content
    sentences = tokenizer.tokenize(content)

    # Replace special characters of sentences and prepare features and labels
    x = [re.sub(reg, '', token) for token in sentences]
    y = [lang for i in range(len(sentences))]
    return x, y

# Prepare 
def output(file, x, y):
    file = open(file, "w+")
    x_temp = np.asarray(x)
    y_temp = np.asarray(y)
    data = np.column_stack((y_temp, x_temp))
    file.writelines(['\t'.join(item)+ '\n' for item in data])
    file.close()

X_Set = [] # Features
Y_Set = [] # Labels
for title in titles:
    # Get features and labels
    x, y = get_content(title[0], title[1])
    X_Set.extend(x)
    Y_Set.extend(y)

# Split features and labels
x_train, x_test, y_train, y_test = train_test_split(X_Set, Y_Set, 
test_size=1/4, random_state=0)

output("train.txt", x_train, y_train)
output("test.txt", x_test, y_test)
# divide to sentences then process everything
# print (get_content("Mustafa Kemal Atatürk", "tr"))