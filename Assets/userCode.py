import UnityEngine
from UnityEngine import *


def spawn():
    global character
    character = gameManager.GetComponent('GameManager').Spawn()


def move(direction):
    if direction == 'Right' or direction == 'right':
        character.GetComponent('Character').MoveRight()
    elif direction == 'Left' or direction == 'left':
        character.GetComponent('Character').MoveLeft()
    else:
        raise ValueError


def attack():
    character.GetComponent('Character').Attack()


def ladderUp():
    character.GetComponent('Character').GoUp()


def ladderDown():
    character.GetComponent('Character').GoDown()


spawn()
for i in range(4):
	move('right')
