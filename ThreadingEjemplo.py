import threading

def Thread1():
    x=0
    while True:
        print(f"Thread1: {x}")
        x+=1

def Thread2():
    x=0
    while True:
        print(f"Thread2: {x}")
        x+=2

t=threading.Thread(target=Thread1)
t.start()
t=threading.Thread(target=Thread2)
t.start()

x=0
while True:
    print(f"Bucle principal: {x}")
    x+=5