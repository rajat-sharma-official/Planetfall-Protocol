INCLUDE globals.ink
-> start

=== start ===
~ register_npc(NPC.marrek)

{ npcs_talked > 2:
    -> normal_hub
- else:
    -> hostile_hub
}

=== hostile_hub ===
Marrek: Not another drifter.
Marrek: If you want comfort, go back down the mountain.

+ [Sorry.] -> END
+ [...] -> END
+ [...] -> END
+ [...] -> END


=== normal_hub ===
You step into <b>Solace Outpost</b> where a rough circle of stone marks their meeting ground.
Marrek is already watching you, arms folded.

Marrek: You’ve been walking this planet long enough to learn one thing—
Marrek: nothing here is free. Not answers. Not help. Not trust.
Marrek: Speak.

-> normal_menu

=== normal_menu ===
+ [Leave.] -> END
+ [What do the Revivalists stand for?] -> normal_belief
+ [What happened to Aurelia’s tech?] -> normal_collapse
+ [Where should I go next?] -> normal_next

=== normal_belief ===
Marrek: We rebuild with what the land can spare.
Marrek: No chasing the <b>old machine</b> like it’s a god.
Marrek: We live. We grow. We bury our dead and keep going.

Marrek: <b>Luminar</b> promises a perfect tomorrow.
Marrek: <b>Solace</b> promises you’ll still be alive when tomorrow comes.
-> normal_menu

=== normal_collapse ===
~ spoke_to_marrek = true
Marrek: Everything ran through one buried system.
Marrek: When it failed, the whole world went dark.
Marrek: Scholars call it tragedy. I call it a warning.

Marrek: They want to “restore” it.
Marrek: We say: some fires don’t need relighting.

Go speak with Scholars...
-> normal_menu

=== normal_next ===
Marrek: If you’re fixing your ship, you’ll need <b>scrap</b>—careful where you take it.
Marrek: If you’re digging for answers… talk to <b>Kase</b>. He keeps track of the ugly truths.

Marrek: And if a scholar smiles too easily at your questions…
Marrek: remember: smiles can be tools.
-> normal_menu