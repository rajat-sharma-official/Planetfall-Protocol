INCLUDE globals.ink
-> start

=== start ===
~ register_npc(NPC.sanya)

{ spoke_to_marrek:
    -> hub_b
- else:
    -> hub_a
}

=== hub_a ===
Sanya: Welcome, traveler.
Sanya: Luminar Outpost rarely receives someone with your experience.
Sanya: If you wish for help, we will give it—gladly.
-> a_menu

=== a_menu ===
+ [Leave.] -> END
+ [What are the scholars trying to do here?] -> a1
+ [What happened to Aurelia’s technology?] -> a2
+ [Are there others on this planet?] -> a3

=== a1 ===
Sanya: We are rebuilding what was lost.
Sanya: The collapse was not our punishment, but our trial.
Sanya: And from trial comes perfection. We will rise again—brighter, stronger, wiser.
Sanya: Stay with us. An explorer like you belongs near a future worth building.
-> a_menu

=== a2 ===
Sanya: The old world depended on systems we no longer fully understand.
Sanya: One failure… and everything went silent.
Sanya: We study the ruins so it never happens again.
Sanya: Not with superstition. With design.
-> a_menu

=== a3 ===
Sanya: There is... They mean well.
Sanya: But they confuse fear with wisdom.
Sanya: They survive day to day—while we prepare a tomorrow that will not collapse under itself.
-> a_menu


=== hub_b ===
Sanya: You’ve been to Solace.
Sanya: I can see it in face.
Sanya: Do not let fear masquerade as truth.
-> b_menu

=== b_menu ===
+ [Leave.] -> END
+ [Why does Solace distrust you so much?] -> b1
+ [Did your research have anything to do with my crash?] -> b2
+ [Why are you so certain restoring tech is right?] -> b3

=== b1 ===
Sanya: Because rebuilding requires courage, and courage looks like madness to the frightened.
Sanya: Solace chooses comfort in limits.
Sanya: We choose responsibility—because someone must.
-> b_menu

=== b2 ===
Sanya: I don’t know.
Sanya: There are storms, anomalies, and forces this planet hides in its geography.
Sanya: If there is a pattern, we have not proven it yet.
Sanya: And we do not build policy on rumor.
-> b_menu

=== b3 ===
Sanya: Because the alternative is slow extinction dressed as humility.
Sanya: The old age failed. Yes.
Sanya: That does not mean knowledge is evil. It means knowledge must mature.
Sanya: Help us. And you will see what “wiser” actually means.
-> b_menu