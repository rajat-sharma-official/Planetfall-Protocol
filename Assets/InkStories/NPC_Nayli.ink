INCLUDE globals.ink
-> start

=== start ===
~ register_npc(NPC.nayli)

{ spoke_to_marrek:
    -> hub_b
- else:
    -> hub_a
}

=== hub_a ===
Nayli: You’re the traveler.
Nayli: Good. I prefer talking to someone who’s actually walked beyond the fences.

Nayli: If you want the world as it is, not as it’s preached—ask me.

-> a_menu

=== a_menu ===
+ [Leave.] -> END
+ [What’s beyond Virelia Frontier?] -> a1
+ [What should I watch for in the Riftlands?] -> a2
+ [Anyone I should worry about?] -> a3

=== a1 ===
Nayli: <b>Echo Basin</b> is quieter—forest cover, short sightlines, easy to get turned around.
Nayli: <b>Riftlands</b> are worse: steep paths, thin air, and places where sound dies.

Nayli: If you want distance, go wide.
Nayli: If you want safety, go slow.
-> a_menu

=== a2 ===
Nayli: Don’t trust “stable ground.”
Nayli: <b>Riftlands</b> likes to crumble when you commit your weight.
Nayli: Mark your routes. Don’t chase shortcuts.
Nayli: And if you’re alone, act like you’re already injured—move like it.
-> a_menu

=== a3 ===
Nayli: People.
Nayli: Not monsters. Not myths. Just people.

Nayli: Watch Danriel in the field—he guards <b>relics</b> like they’re holy.
Nayli: Watch Harvel in the workshop—he’ll cut you with words before you ever touch a tool.
Nayli: And watch Sanya when she smiles.
Nayli: Leaders don’t smile for nothing.
-> a_menu


=== hub_b ===
Nayli: You’ve heard Marrek.
Nayli: Fair.
Nayli: Just remember: leaders speak in absolutes because it keeps people moving.

-> b_menu

=== b_menu ===
+ [Leave.] -> END
+ [Do you think scholars are hiding something?] -> b1
+ [Have you noticed anything strange in the environment?] -> b2
+ [What’s the real difference between Solace and Luminar?] -> b3

=== b1 ===
Nayli: I think the outpost chooses what it says out loud.
Nayli: That’s not proof of a conspiracy. It’s just… leadership.
Nayli: But yes. Sometimes our answers are too clean.

Nayli: Clean answers usually mean the messy parts were cut out.
-> b_menu

=== b2 ===
Nayli: There are spots where compasses drift and machines act wrong.
Nayli: Not everywhere. Not consistent enough to “prove.”
Nayli: But enough to keep me careful.

Nayli: The land feels old.
Nayli: Like it remembers being used.
-> b_menu

=== b3 ===
Nayli: <b>Solace</b> tries to survive without waking the past.
Nayli: <b>Luminar</b> tries to resurrect the past and call it a future.
Nayli: Both think they’re saving Aurelia.
Nayli: Both could be wrong in different ways.

Nayli: But Solace is honest about pain.
Nayli: Luminar is… better at speeches.
-> b_menu