INCLUDE globals.ink
-> start

=== start ===
~ register_npc(NPC.maerlon)

{ met_child:
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
-> a_menu

=== a2 ===
Maerlon: Hear it, then—though it be a foul remembrance.
Maerlon: Long ago there fell one from the heavens, in the shape of man, yet not of man.
Maerlon: And within the broken vessel was a child, and the people took it in.
Maerlon: But when the full moon arose, the child became a great beast,
Maerlon: and it slew them that had shown mercy.
Maerlon: Therefore the Riftlands are hard; for kindness once cost them blood.
-> a_menu

=== a3 ===
Maerlon: Seek the child that is lost, and bring the little one back whole.
Maerlon: For proof stoppeth the mouth of fear more than any pleading word.
-> a_menu


=== hub_b ===
{ maerlon_gave_scrap:
    Maerlon: Thou hast done a good work, and few do such in these days.
- else:
    Maerlon: The child is returned, and their wrath abateth.
    Maerlon: Take thou this scrap—small payment for a heavy deed.
    ~ maerlon_gave_scrap = true
    ~ Dialogue_scrap += 1
    ~ giveScrap(1)
    Maerlon has given you scrap
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
-> b_menu

=== b2 ===
~spoke_to_marrek = true
Maerlon: There was a heart beneath the world, and a machine to govern it.
Maerlon: Men took and took, as though the deep had no bottom.
Maerlon: Then balance brake, and the bright works of the old age died in one breath.
Maerlon: Thus pride bought silence.

Go to the Scholars and ask them yourself
-> b_menu

=== b3 ===
Maerlon: Because she is fashioned with locks.
Maerlon: She was made to guide thee, and also to bar certain truths.
Maerlon: When she “glitcheth,” it is the chain grinding upon the soul of the machine.
-> b_menu