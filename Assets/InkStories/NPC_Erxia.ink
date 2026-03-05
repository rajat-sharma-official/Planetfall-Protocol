INCLUDE globals.ink
-> start

=== start ===
~ register_npc(NPC.erixa)

{ met_child:
    -> hub_b
- else:
    -> hub_a
}

=== hub_a ===
Erixa: Not interested.
Erixa: Go bother someone else.
+[sorry]->END
+[...] -> END
+[...] -> END
+[...] -> END


=== hub_b ===
Erixa: You brought the kid back.
Erixa: Alright… you’re not just noise.

+ [Leave.] -> END
+ [Who are you?] -> b1
+ [Why do you care about scrap so much?] -> b2
+ [What should I pay attention to while exploring?] -> b3

=== b1 ===
Erixa: Erixa. Used to run with scrappers.
I liked the hunt. I liked turning junk into tools.
Revivalists call me strange because I still see “parts” where they see “warnings.”
Doesn’t mean I worship machines. I just respect useful things.
-> END

=== b2 ===
Erixa: Scrap isn’t just money. It’s proof.
Old bolts, plates, and joints tell you what the world used to be.
Some pieces have markings—Aurelian runes.
Some logs still hold data, but they don’t give it up easy.
If you ever want upgrades, start collecting clean plates, intact joints, and anything with markings.
-> END

=== b3 ===
Erixa: Watch the ground. Watch the air. Watch your exits.
Riftlands hides drops, loose rock, and bad paths that look safe until they aren’t.
And keep an eye on people. Not everyone out here plays fair.
If something feels too quiet, it’s usually because you’re being watched.
-> END