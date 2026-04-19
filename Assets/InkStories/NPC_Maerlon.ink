INCLUDE globals.ink
-> start

=== start ===
~ register_npc(NPC.maerlon)

{ npcs_talked > 2:
    -> hub_b
- else:
    -> hub_a
}

=== hub_a ===
Maerlon: Behold, a stranger walketh the Riftlands.
Maerlon: Thou art a sign unto them, and their hearts fail for fear.
-> a_menu

=== a_menu ===
+ [Leave.] -> END
+ [Wherefore do they revile me?] -> a1
+ [Speak plainly—what tale bindeth them?] -> a2
+ [How may I turn away their wrath?] -> a3

=== a1 ===
Maerlon: They behold not thy deeds, but thy likeness.
Maerlon: For an old terror wearied their souls, and now they see its shadow upon thee.
Maerlon: Thou hast come among a people taught by grief to fear first and judge after.
Maerlon: It is not justice that meeteth thee, but remembrance.
-> a_menu

=== a2 ===
Maerlon: Riftlands are hard; for kindness once cost them blood.
Maerlon: In the elder days, fair words were the cloak of cruel men.
Maerlon: Promises were made, and afterward came hunger, chains, and fire.
Maerlon: Thus the people learned to shut the door before they looked upon the guest.

Maerlon: Yet hear me—fear is a poor prophet.
Maerlon: It speaketh loudly, and it speaketh early,
Maerlon: but it rarely speaketh true.
-> a_menu

=== a3 ===
Maerlon: Walk softly.
Maerlon: Ask little, take less, and repay what thou art given.
Maerlon: For trust is not won by speech, but by habit.

Maerlon: And if thou wouldst be understood,
Maerlon: be found doing good when none command thee.
-> a_menu


=== hub_b ===
{ maerlon_gave_scrap:
    Maerlon: Thou hast done a good work, and few do such in these days.
- else:
    Maerlon: Take thou this scrap—small payment for a heavy deed.
    ~ maerlon_gave_scrap = true
    ~ Dialogue_scrap += 1
    ~ giveScrap(1)
    Maerlon has given you scrap.
}
-> b_menu

=== b_menu ===
+ [Leave.] -> END
+ [Was my crash mere chance?] -> b1
+ [What brought the great collapse upon Aurelia?] -> b2
+ [Why doth VERA falter and conceal?] -> b3

=== b1 ===
Maerlon: Chance? Nay.
Maerlon: There be old snares beneath this earth—half-dead defenses that still hunger.
Maerlon: They draw passing ships downward, as a net draweth the fish from the deep.
Maerlon: Long after their makers perished, their purpose endureth like a curse.
Maerlon: Thou wert not chosen by fate, perhaps—but neither wert thou wholly free of design.
-> b_menu

=== b2 ===
~spoke_to_marrek = true
Maerlon: There was a heart beneath the world, and a machine to govern it.
Maerlon: Men took and took, as though the deep had no bottom.
Maerlon: Then balance brake, and the bright works of the old age died in one breath.
Maerlon: Thus pride bought silence.

Maerlon: Go to the scholars and ask them thyself.
Maerlon: Mark well how they answer when truth draweth near.
-> b_menu

=== b3 ===
Maerlon: Because she is fashioned with locks.
Maerlon: She was made to guide thee, and also to bar certain truths.
Maerlon: She knoweth more than she may speak, and suffereth for the knowing.
Maerlon: Pity her, if thou canst; for a bound mind is a sorrowful thing.
-> b_menu