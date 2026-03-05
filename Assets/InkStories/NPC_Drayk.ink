INCLUDE globals.ink
-> start

=== start ===
~ register_npc(NPC.drayk)

{ met_child:
    -> hub_b
- else:
    -> hub_a
}

=== hub_a ===
Drayk: Don’t loiter.
Drayk: You’re in the way.
+[sorry]->END
+[...] -> END
+[...] -> END
+[...] -> END






=== hub_b ===
Drayk: You brought the kid back.
Drayk: Means you can be useful. That’s rare.

+ [Leave.] -> END
+ [What do you do here?] -> b1
+ [I need scrap—can I scavenge in the Riftlands?] -> b2
+ [How does Solace survive out here?] -> b3

=== b1 ===
Drayk: I run resources. Food, water, shelter materials, work crews.
People think “leader” means speeches.
Up here it means counting what you have and deciding who gets it.
I don’t like strangers because strangers are risk.
-> END

==== b2 ===
{ drayk_gave_scrap:
    Drayk: I already told you where to look. Don’t come back asking for handouts.
- else:
    Drayk: You want scrap? Take it.
    Drayk: Not from our stores—only from dead frames and abandoned piles outside the outpost.
    Drayk: We don’t build our lives on dead machines. If you do, that’s your business.
    Drayk: Just don’t bring trouble back here.
    ~ drayk_gave_scrap = true
    ~ Dialogue_scrap += 1
    ~ giveScrap(1)
}
-> END
=== b3 ===
Drayk: Rules. Discipline. Work.
We don’t waste. We don’t show off. We don’t pretend we’re invincible.
You fall behind out here, the mountain doesn’t care who you are.
That’s why we plan like we’re always one mistake away from hunger.
-> END