INCLUDE globals.ink
-> start

=== start ===
~ register_npc(NPC.kase)

{ npcs_talked > 2:
    -> hub_b
- else:
    -> hub_a
}

=== hub_a ===
Kase: Don’t waste my time.
Kase: I’m working.

+ [Sorry.] -> END
+ [...] -> END
+ [...] -> END
+ [...] -> END


=== hub_b ===
Kase sits apart from the main path, surrounded by scraps of notes and charcoal sketches of old mechanisms.

Kase: Speak.
Kase: And if you want comfort, go talk to someone who sells it.

-> b_menu

=== b_menu ===
+ [Leave.] -> END
+ [Who are you?] -> b1
+ [What caused the collapse?] -> b2
+ [What do you fear the scholars will do?] -> b3

=== b1 ===
Kase: Kase. Researcher.
I write down the parts everyone avoids: cause and consequence.
<b>Solace</b> survives because we learn. Not because we hope.
Hope is for people with safety.

Kase: Most people want stories.
Kase: I want mechanisms.
-> b_menu

=== b2 ===
Kase: One buried <b>system</b> fed the whole “advanced” age.
Power, distribution, regulation—everything ran through it.
Then it <b>failed</b>, and the world’s tech died together.
Not “slowly.” Not “over time.” Together.

Kase: If you want meaning, you’ll find it later.
Kase: First you need the shape of the truth.
-> b_menu

=== b3 ===
Kase: If they find the <b>old system</b>, they’ll try to restart it.
They’ll tell themselves it’ll be different this time.
It never is.
The planet paid once already.
I won’t help anyone make it pay twice.

Kase: Scholars talk about “saving” Aurelia.
Kase: Sometimes saving looks a lot like taking.
-> b_menu