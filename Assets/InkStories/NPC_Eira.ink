INCLUDE globals.ink
-> start

=== start ===
~ register_npc(NPC.eira)

{ npcs_talked > 2:
    -> hub_b
- else:
    -> hub_a
}

=== hub_a ===
Eira: Not today.
Eira: I’m busy. Move.

+ [Sorry.] -> END
+ [...] -> END
+ [...] -> END
+ [...] -> END


=== hub_b ===
You find Eira near medical herbs, her hands stained with crushed leaves.
She glances up just once—long enough to measure how tired you look—then motions you closer.

Eira: Sit if you’re hurt. Stand if you’re lying.
Eira: I don’t have time for pride.

-> b_menu

=== b_menu ===
+ [Leave.] -> END
+ [Who are you, really?] -> b1
+ [Can you help if I’m hurt?] -> b2
+ [What is Solace Outpost trying to be?] -> b3

=== b1 ===
Eira: I’m Eira. I patch up what the Riftlands breaks.
Eira: People think healing is gentle work.
Eira: It isn’t. It’s stubborn work.

Eira: I stay calm so other people can keep moving.
Eira: If I panic, the outpost panics.
Eira: And then someone dies for no reason.
-> b_menu

=== b2 ===
Eira: Yes. Sit down and tell the truth—where it hurts, how bad, how long.
Eira: The Riftlands don’t kill you with one dramatic blow.
Eira: They take you piece by piece—cold, hunger, bad footing, exhaustion.

Eira: Come early. Healing works best before you’re desperate.
Eira: Don’t wait until your body is bargaining with you.
-> b_menu

=== b3 ===
Eira: Solace is a promise.
Eira: Not that life will be easy—just that we’ll stop pretending the old world is coming back.
Eira: Luminar talks about rebuilding machines.
Eira: We rebuild people.

Eira: I want the colonies to stop acting like strangers forever.
Eira: But first… we have to stop talking like we’re different species.
-> b_menu