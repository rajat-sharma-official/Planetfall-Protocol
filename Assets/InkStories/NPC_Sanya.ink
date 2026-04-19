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
Sanya: You look like someone who still believes the universe is orderly.
Sanya: That belief is useful. Keep it.

Sanya: If you wish for help, we will give it—gladly.
Sanya: You are not alone here.

-> a_menu

=== a_menu ===
+ [Leave.] -> END
+ [What are the scholars trying to do here?] -> a1
+ [What happened to Aurelia’s technology?] -> a2
+ [Are there others on this planet?] -> a3

=== a1 ===
Sanya: We are rebuilding what was lost.
Sanya: The collapse was not our punishment, but our trial.
Sanya: And from trial comes perfection.
Sanya: We will rise again—brighter, stronger, wiser.

Sanya: You are a planetary explorer. You understand systems. Logistics. Survival.
Sanya: Stay with us. People like you do not belong in the dirt with broken tools.
Sanya: You belong near a future worth building.

-> a_menu

=== a2 ===
Sanya: The old world depended on systems we no longer fully understand.
Sanya: One failure… and everything went silent.
Sanya: Not just lights. Not just machines.
Sanya: Entire chains of knowledge snapped, like thread burned through.

Sanya: We study the ruins so it never happens again.
Sanya: Not with fear. Not with folk remedies.
Sanya: With design. With discipline. With proof.

-> a_menu

=== a3 ===
Sanya: There are others, yes.
Sanya: Some build. Some hide. Some wander and call it wisdom.
Sanya: They mean well—most of them.
Sanya: But they confuse fear with safety.

Sanya: We do not insult them. We… pity them.
Sanya: A child clings to the dark because the dark feels familiar.
Sanya: We are teaching Aurelia to be bright again.

-> a_menu


=== hub_b ===
Sanya: You’ve been to Solace.
Sanya: I can see it in your face.
Sanya: Do not let fear masquerade as truth.

Sanya: They speak as though suffering is a virtue.
Sanya: Suffering is not a virtue. It is a problem.

-> b_menu

=== b_menu ===
+ [Leave.] -> END
+ [Why does Solace distrust you so much?] -> b1
+ [Did your research have anything to do with my crash?] -> b2
+ [Why are you so certain restoring tech is right?] -> b3

=== b1 ===
Sanya: Because rebuilding requires courage.
Sanya: And courage looks like madness to the frightened.
Sanya: Solace chooses comfort in limits.
Sanya: We choose responsibility—because someone must.

Sanya: If they could truly see what we are building, they would beg to join us.
Sanya: But it is easier to call the future dangerous than to admit you are afraid of it.

-> b_menu

=== b2 ===
Sanya: I don’t know.
Sanya: There are storms, anomalies, and forces this planet hides in its geography.
Sanya: If there is a pattern, we have not proven it yet.
Sanya: And we do not build policy on rumor.

Sanya: Let me guess—now you think every shadow is a conspiracy.
Sanya: Please. Breathe.
Sanya: You crashed. You survived. Focus on what can be built.

-> b_menu

=== b3 ===
Sanya: Because the alternative is slow extinction dressed as humility.
Sanya: The old age failed. Yes.
Sanya: That does not mean knowledge is evil.
Sanya: It means knowledge must mature.

Sanya: We will not repeat the old mistake.
Sanya: We will build redundancies. Safeguards. Constraints.
Sanya: We will make “collapse” a word for history books—nothing more.

-> b_menu