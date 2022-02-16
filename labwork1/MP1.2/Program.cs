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
            (string, int[], int)[] InfoWords = new (string, int[], int)[10];
            int currentLengthInfoWords = 0;
            int lengthInfoWords = 10;
            int numberOfLine = 0;

            string path = "input.txt";
            string word = "";
            using (StreamReader reader = new StreamReader(path))
            {
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
                        if (temp == '\n')
                        {
                            numberOfLine++;
                        }
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
                        
                        int count = 0;
                        SearchWord:
                        if (count != currentLengthInfoWords)
                        {
                            if (InfoWords[count].Item1 == word)
                            {
                                if (InfoWords[count].Item3 <= 100)
                                {
                                    InfoWords[count].Item2[InfoWords[count].Item3] = numberOfLine / 45 + 1;
                                    InfoWords[count].Item3++;
                                }

                                word = "";
                                goto ReadChar;
                            }
                            count++;
                            goto SearchWord;
                        }
                        
                        int[] pages = new int[101];
                        pages[0] = numberOfLine / 45 + 1;
                        
                        InfoWords[currentLengthInfoWords] = (word, pages, 1);
                        word = "";
                        currentLengthInfoWords++;
                    }
                    
                    if (lengthInfoWords*0.8 <= currentLengthInfoWords) //expand array
                    {
                        (string, int[], int)[] tempInfoWords = InfoWords;
                        int currentIndex = 0;
                        lengthInfoWords *= 2;
                        InfoWords = new (string, int[], int)[lengthInfoWords];
                        CopyArray1:
                            InfoWords[currentIndex] = tempInfoWords[currentIndex];
                            currentIndex++;
                            if (currentIndex != currentLengthInfoWords)
                            {
                                goto CopyArray1;
                            }
                    }
                    if (!reader.EndOfStream)
                        goto ReadChar;

            }

            int index1 = 0;
            int index2 = 0;
            Sort:
            int charIndex = 0;
            int lengthCharIndex;
            bool swap=false;

            if (InfoWords[index1].Item1.Length<InfoWords[index2].Item1.Length)
            {
                lengthCharIndex = InfoWords[index1].Item1.Length;
            }
            else
            {
                lengthCharIndex = InfoWords[index2].Item1.Length;
                swap=true;
            }
            
            CompareTwoWorld:
                if (InfoWords[index1].Item1[charIndex] > InfoWords[index2].Item1[charIndex])
                {
                    swap = true;
                }
                else if (InfoWords[index1].Item1[charIndex] < InfoWords[index2].Item1[charIndex])
                {
                    swap = false;
                }
                else
                {
                    charIndex++;
                    if (charIndex < lengthCharIndex)
                    {
                        goto  CompareTwoWorld;
                    }
                }

            if (swap)
            {
                (string, int[], int) temp = InfoWords[index1];
                InfoWords[index1] = InfoWords[index2];
                InfoWords[index2] = temp;
            }

            index2++;
            if (index2 == currentLengthInfoWords)
            {
                index1++;
                index2 = index1;
            }
            if(index1 != currentLengthInfoWords)
                goto Sort;

            using StreamWriter writer = new StreamWriter("output.txt");
            int countOutput = 0;
            int countWord = 0;
            OutputWord:
            if (currentLengthInfoWords != countOutput)
            {
                if (InfoWords[countOutput].Item3 == 101)
                {
                    countOutput++;
                    goto OutputWord;
                }
                writer.Write($"{InfoWords[countOutput].Item1} - ");
                
                OutputPage:
                if (countWord != InfoWords[countOutput].Item3 - 1)
                {
                    writer.Write($"{InfoWords[countOutput].Item2[countWord]}, ");
                    countWord++;
                    goto OutputPage;
                }
                
                writer.Write($"{InfoWords[countOutput].Item2[InfoWords[countOutput].Item3 - 1]}");
                writer.WriteLine();

                countWord = 0;
                countOutput++;
                goto OutputWord;
            }
        }
    }
}