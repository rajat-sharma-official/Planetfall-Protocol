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
-> a_menu

=== a_menu ===
+ [Leave.] -> END
+ [What’s beyond Virelia Frontier?] -> a1
+ [What should I watch for in the Riftlands?] -> a2
+ [Anyone I should worry about?] -> a3

=== a1 ===
Nayli: Echo Basin is quieter—forest cover, short sightlines, easy to get turned around.
Riftlands are worse: steep paths, thin air, and places where sound dies.
If you want distance, go wide. If you want safety, go slow.
-> a_menu

=== a2 ===
Nayli: Don’t trust “stable ground.”
Riftlands likes to crumble when you commit your weight.
Mark your routes. Don’t chase shortcuts.
And if you’re alone, act like you’re already injured—move like it.
-> a_menu

=== a3 ===
Nayli: ...
They chose a different way to hope.
Sanya doesn’t like that because it threatens the scholar narrative.
But I’ve met decent people in Solace. Real people. Not enemies.
-> a_menu


=== hub_b ===
Nayli: You’ve heard Marrek.
Fair.
Just remember: leaders speak in absolutes because it keeps people moving.
-> b_menu

=== b_menu ===
+ [Leave.] -> END
+ [Do you think scholars are hiding something?] -> b1
+ [Have you noticed anything strange in the environment?] -> b2
+ [What’s the real difference between Solace and Luminar?] -> b3

=== b1 ===
Nayli: I think the outpost chooses what it says out loud.
That’s not proof of a conspiracy. It’s just… leadership.
But yes. Sometimes our answers are too clean.
-> b_menu

=== b2 ===
Nayli: There are spots where compasses drift and machines act wrong.
Not everywhere. Not consistent enough to “prove.”
But enough to keep me careful.
If you feel like the land is pulling you… don’t ignore it.
-> b_menu

=== b3 ===
Nayli: Solace tries to survive without waking the past.
Luminar tries to resurrect the past and call it a future.
Both think they’re saving Aurelia.
Both could be wrong in different ways.
-> b_menu