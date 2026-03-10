INCLUDE globals.ink
-> start

=== start ===
~ register_npc(NPC.marrek)

{ met_child:
    -> normal_hub
- else:
    -> hostile_hub
}

=== hostile_hub ===
Marrek: Not another drifter....
+[sorry]->END
+[...] -> END
+[...] -> END
+[...] -> END


=== normal_hub ===
You step into the center of Solace Outpost where a rough circle of stone marks their meeting ground.
Marrek is already watching you, arms folded, like he’s been expecting you to speak first.
Marrek: You brought the child back.
Marrek: Alright. You’re not dead weight.
->normal_menu

=== normal_menu === 
+ [Leave.] -> END
+ [What do the Revivalists stand for?] -> normal_belief
+ [What happened to Aurelia’s tech?] -> normal_collapse
+ [Where should I go next?] -> normal_next

=== normal_belief ===
Marrek: We rebuild with what the land can spare.
Marrek: No chasing the old machine like it’s a god.
Marrek: We live. We grow. That’s it.
-> normal_menu

=== normal_collapse ===
~ spoke_to_marrek = true
Marrek: Everything ran through one buried system.
Marrek: When it failed, the whole world went dark.
Marrek: Scholars call it tragedy. I call it a warning.

Go speak with Scholars...
-> normal_menu

=== normal_next ===
Marrek: If you’re fixing your ship, you’ll need scrap—careful where you take it.
Marrek: And if you’re digging for answers… talk to Kase. He keeps track of the ugly truths.
Marrek: I believe Drayk has some scrap that might be of use to you
-> normal_menu