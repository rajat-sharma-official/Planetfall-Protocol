INCLUDE globals.ink
-> start

=== start ===
~ register_npc(NPC.eira)

{ met_child:
    -> hub_b
- else:
    -> hub_a
}

=== hub_a ===
Eira: Not today.
Eira: I’m busy. Move.
+[sorry]->END
+[...] -> END
+[...] -> END
+[...] -> END



=== hub_b ===
You find Eira near medical herbs, her hands stained with crushed leaves.
She glances up just once—long enough to measure how tired you look—then motions you closer.
->b_menu

=== b_menu ===
+ [Leave.] -> END
+ [Who are you, really?] -> b1
+ [Can you help if I’m hurt?] -> b2
+ [What is Solace Outpost trying to be?] -> b3

=== b1 ===
Eira: I’m Eira. I patch up what the Riftlands breaks.
Eira: I wasn’t raised for this. Nobody is.
Eira: But when your home is far from help, you become the help.
I stay calm so other people can keep moving.
-> b_menu

=== b2 ===
Eira: Yes. Sit down and tell the truth—where it hurts, how bad, how long.
Eira: The Riftlands don’t kill you with one dramatic blow.
They take you piece by piece—cold, hunger, bad footing, exhaustion.
Come early. Healing works best before you’re desperate.
-> b_menu

=== b3 ===
Eira: Solace is a promise.
Not that life will be easy—just that we’ll stop pretending the old world is coming back.
The scholars want restoration. The scrappers want profit. We want survival without repeating the same mistake.
And I want the colonies to stop acting like strangers forever.
-> b_menu