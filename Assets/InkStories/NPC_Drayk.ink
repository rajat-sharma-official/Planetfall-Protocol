INCLUDE globals.ink
-> start

=== start ===
~ register_npc(NPC.drayk)

{ npcs_talked > 2:
    -> hub_b
- else:
    -> hub_a
}

=== hub_a ===
Drayk: Don’t loiter.
You’re in the way.

+ [Sorry.] -> END
+ [...] -> END
+ [...] -> END
+ [...] -> END


=== hub_b ===
Drayk: It seems like you can be useful.
Drayk: That’s rare.

Drayk: I don’t like strangers.
Drayk: Strangers are risk.
Drayk: But risk can be managed if you follow rules.

-> b_menu

=== b_menu ===
+ [Leave.] -> END
+ [What do you do here?] -> b1
+ [I need scrap—can I scavenge in the Riftlands?] -> b2
+ [How does Solace survive out here?] -> b3

=== b1 ===
Drayk: I run resources. Food, water, shelter materials, work crews.

Drayk: Everyone wants to be a hero.
Drayk: Nobody wants to carry the inventory when winter hits.
-> b_menu

=== b2 ===
{ drayk_gave_scrap:
    Drayk: I already told you where to look. Don’t come back asking for handouts.
    Drayk: The mountain doesn’t hand you anything. Neither do I.
- else:
    Drayk: You want <b>scrap</b>? Take it.
    Drayk: We don’t build our lives on <b>dead machines</b>. If you do, that’s your business.

    Drayk: This is me helping you once.
    Drayk: Don’t make me regret it.
    ~ drayk_gave_scrap = true
    ~ Dialogue_scrap += 1
    ~ giveScrap(1)
    Drayk has given you scrap.
}
-> b_menu

=== b3 ===
Drayk: Rules. Discipline. Work.
We don’t waste. We don’t show off. We don’t pretend we’re invincible.
You fall behind out here, the mountain doesn’t care who you are.

Drayk: <b>Solace</b> survives because we plan like we’re always one mistake away from hunger.
Because we are.
-> b_menu