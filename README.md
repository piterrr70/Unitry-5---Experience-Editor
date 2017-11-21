# Unitry 5 - Experience Editor


This is Experience Editor based on Scriptable Objects Example be Unity.

### Features:
* Random Generator (is set to 107 items on list).

### Ovewview

![alt text](https://i.imgur.com/nN4n5SV.png)


### Editing:
You can change count of generated items in:
```
void GenerateDefault()
        {
            int last = 106;   <=== now is 106 + 1 first item 
            for(int i = 1; i <= last; i++)
            {
                AddItem();
            }
        }
```