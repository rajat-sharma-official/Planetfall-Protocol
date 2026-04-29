INCLUDE globals.ink
-> start

=== start ===
~ register_npc(NPC.erixa)

{ npcs_talked > 2:
    -> hub_b
- else:
    -> hub_a
}

=== hub_a ===
Erixa: Not interested.
Erixa: Go bother someone else.

+ [Sorry.] -> END
+ [...] -> END
+ [...] -> END
+ [...] -> END


=== hub_b ===
Erixa: Alright… you’re not just noise.
Erixa: You keep showing up. That counts for something.

-> b_menu

=== b_menu ===
+ [Leave.] -> END
+ [Who are you?] -> b1
+ [Why do you care about scrap so much?] -> b2
+ [What should I pay attention to while exploring?] -> b3

=== b1 ===
Erixa: Erixa. Used to run with <b>scrappers</b>.
I liked the hunt. I liked turning junk into tools.
Revivalists call me strange because I still see “parts” where they see “warnings.”

Erixa: The world is full of broken stuff.
Erixa: Most people only see “broken.”
-> b_menu

=== b2 ===
Erixa: <b>Scrap</b> isn’t just money. It’s proof.
Old bolts, plates, and joints tell you what the world used to be.
Some pieces have markings—<b>Aurelian runes</b>.

Erixa: And sometimes you find stuff that doesn’t feel like scrap.
Like somebody meant it to be found.
A note etched into a plate. A symbol scratched under a seam.

Erixa: If you ever want upgrades, start <b>collecting</b> clean plates, intact joints, and anything with markings.
-> b_menu

=== b3 ===
Erixa: Watch the ground. Watch the air. Watch your exits.
<b>Riftlands</b> hides drops, loose rock, and bad paths that look safe until they aren’t.

Erixa: <b>Fog</b> rolls in fast up here.
If your world suddenly feels smaller, treat it like a <b>warning</b>.
And keep an eye on people. Not everyone out here plays fair.
If something feels too quiet, it’s usually because you’re being watched.
-> b_menu