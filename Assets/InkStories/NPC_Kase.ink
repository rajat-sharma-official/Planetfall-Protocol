INCLUDE globals.ink
-> start

=== start ===
~ register_npc(NPC.kase)

{ met_child:
    -> hub_b
- else:
    -> hub_a
}

=== hub_a ===
Kase: Don’t waste my time.
Kase: I’m working.
+[sorry]->END
+[...] -> END
+[...] -> END
+[...] -> END


=== hub_b ===
Kase sits apart from the main path, surrounded by scraps of notes and charcoal sketches of old mechanisms.
He doesn’t greet you—he just pauses mid-writing, waiting to see if you’re worth the interruption

+ [Leave.] -> END
+ [Who are you?] -> b1
+ [What caused the collapse?] -> b2
+ [What do you fear the scholars will do?] -> b3

=== b1 ===
Kase: Kase. Researcher.
I write down the parts everyone avoids: cause and consequence.
Solace survives because we learn. Not because we hope.
Hope is for people with safety.
-> END

=== b2 ===
Kase: One buried system fed the whole “advanced” age.
Power, distribution, regulation—everything ran through it.
Then it failed, and the world’s tech died together.
Not “slowly.” Not “over time.” Together.
Like a body when the heart stops.
-> END

=== b3 ===
Kase: If they find the old system, they’ll try to restart it.
They’ll tell themselves it’ll be different this time.
It never is.
The planet paid once already.
I won’t help anyone make it pay twice.
-> END