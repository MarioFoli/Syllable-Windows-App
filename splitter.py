import spacy
import sys
iniWord = sys.argv[1]

from spacy_syllables import SpacySyllables
nlp = spacy.load('en_core_web_md')
syllables = SpacySyllables(nlp)
nlp.add_pipe('syllables', after='tagger')


def spacy_syllablize(word):
    token = nlp(word)[0]
    return token._.syllables


for test_word in [iniWord]:
    print(f"{test_word} -> {spacy_syllablize(test_word)}")
