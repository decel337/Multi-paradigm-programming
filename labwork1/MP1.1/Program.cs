using System.IO;

namespace MP1._1
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] stopWord = new[] { "i", "me", "my", "myself", "we", "our", "ours", "ourselves", "you", "your", 
                "yours", "yourself", "yourselves", "he", "him", "his", "himself", "she", "her", "hers", "herself", 
                "it", "its", "itself", "they", "them", "their", "theirs", "themselves", "what", "which", "who", "whom", 
                "this", "that", "these", "those", "am", "is", "are", "was", "were", "be", "been", "being", "have", "has", 
                "had", "having", "do", "does", "did", "doing", "a", "an", "the", "and", "but", "if", "or", "because", 
                "as", "until", "while", "of", "at", "by", "for", "with", "about", "against", "between", "into", "through", 
                "during", "before", "after", "above", "below", "to", "from", "up", "down", "in", "out", "on", "off", "over", 
                "under", "again", "further", "then", "once", "here", "there", "when", "where", "why", "how", "all", "any", 
                "both", "each", "few", "more", "most", "other", "some", "such", "no", "nor", "not", "only", "own", "same", 
                "so", "than", "too", "very", "s", "t", "can", "will", "just", "don", "should", "now" };
            
            string[] words = new string[10];
            int lengthWords = 10;
            int currentLengthWords = 0;
            string path = "engl.txt";
            using (StreamReader reader = new StreamReader(path))
            {
                string word = "";
                ReadChar:
                    char temp = (char)reader.Read();

                    if (temp >= 'A' && temp <= 'Z')
                        temp += (char)32; //remove capital letters

                    if (temp >= 'a' && temp <= 'z')
                    {
                        word = word + temp;
                        goto ReadChar;
                    }

                    if (temp == ' ' || temp == '\n') //is end of the word 
                    {
                        if (word == "")
                        {
                            goto ReadChar;
                        }
                        int indexStopWord = 0;
                        CheckStopWord:
                        if (indexStopWord != stopWord.Length)
                        {
                            if (stopWord[indexStopWord] == word)
                            {
                                word = "";
                                goto ReadChar;
                            }

                            indexStopWord++;
                            goto CheckStopWord;
                        }
                        
                        words[currentLengthWords] = word;
                        word = "";
                        currentLengthWords++;
                    }
                    
                    if (lengthWords*0.8 <= currentLengthWords) //expand array
                    {
                        string[] tempWords = words;
                        int currentIndex = 0;
                        lengthWords *= 2;
                        words = new string[lengthWords];
                        CopyArray1:
                            words[currentIndex] = tempWords[currentIndex];
                            currentIndex++;
                            if (currentIndex != currentLengthWords)
                            {
                                goto CopyArray1;
                            }
                            goto ReadChar;
                    }
                    
                    
                    if (!reader.EndOfStream)
                    {
                        goto ReadChar;
                    }
            }
            
            (string, int)[] numberOfWords = new (string, int)[10];

            numberOfWords[0] = (words[0], 1);
            int currentLengthNumberOfWords = 1;
            int lengthNumberOfWords = 10;
            int wordIndex = 1;
            int i = 0;
            CheckWithAllSavedWords:
                if (words[wordIndex] != numberOfWords[i].Item1 && wordIndex != currentLengthWords)
                {
                    i++;
                    if (currentLengthNumberOfWords == i)
                    {
                        numberOfWords[currentLengthNumberOfWords] = (words[wordIndex], 1);
                        currentLengthNumberOfWords++;
                        i = 0;
                        wordIndex++;
                    }
                    
                    if (lengthNumberOfWords*0.8 <= currentLengthNumberOfWords) //expand array
                    {
                        (string, int)[] tempWords = numberOfWords;
                        int currentIndex = 0;
                        lengthNumberOfWords *= 2;
                        numberOfWords = new (string, int)[lengthNumberOfWords];
                        CopyArray2:
                        numberOfWords[currentIndex] = tempWords[currentIndex];
                        currentIndex++;
                        if (currentIndex != currentLengthNumberOfWords)
                        {
                            goto CopyArray2;
                        }
                    }
                    goto CheckWithAllSavedWords;
                }
                if(words[wordIndex] == numberOfWords[i].Item1)
                {
                    numberOfWords[i].Item2++;
                    i = 0;
                    wordIndex++;
                    goto CheckWithAllSavedWords;
                }

                int index1 = 0;
                int index2 = 0;
                Sort:
                if (numberOfWords[index1].Item2 < numberOfWords[index2].Item2)
                {
                    (string, int) temp = numberOfWords[index1];
                    numberOfWords[index1] = numberOfWords[index2];
                    numberOfWords[index2] = temp;
                }

                index2++;
                if (index2 == currentLengthNumberOfWords)
                {
                    index1++;
                    index2 = index1;
                    goto Sort;
                }
                if(index1 != currentLengthNumberOfWords)
                    goto Sort;
                
                using StreamWriter writer = new StreamWriter("output.txt");
                int count = 0;
                OutputNumberOfWord:
                if (currentLengthNumberOfWords != count)
                {
                    writer.WriteLine($"{numberOfWords[count].Item1} - {numberOfWords[count].Item2} repeat");
                    count++;
                    goto OutputNumberOfWord;
                }
        }
    }
    }