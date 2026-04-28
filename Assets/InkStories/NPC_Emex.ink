INCLUDE globals.ink
-> start

=== start ===
~ register_npc(NPC.emex)

{ spoke_to_marrek:
    -> hub_b
- else:
    -> hub_a
}

=== hub_a ===
Emex: You’re new.
Emex: You still think <b>Luminar</b> smiles because it cares.
Emex: Give it time. The smile starts to feel like a mask.

-> a_menu

=== a_menu ===
+ [Leave.] -> END
+ [Do you hate them?] -> a1
+ [Are the scholars lying to me?] -> a2
+ [Where are you going?] -> a3

=== a1 ===
Emex: “Hate” is easy.
Emex: What I feel is… tired.

Emex: <b>Luminar</b> shines with knowledge and hope—so bright everyone in it is blind.
Emex: If you don’t worship the mission, you’re treated like a defect.
Emex: You become a problem they’re too polite to name.
-> a_menu

=== a2 ===
Emex: They’re not liars in the simple way.
Emex: They believe their story so hard they stop seeing what doesn’t fit.

Emex: And when you ask the wrong question, they smile and say, “I don’t know.”
Emex: Not because they can’t answer.
Emex: Because the answer would make you inconvenient.
-> a_menu

=== a3 ===
Emex: <b>Solace</b>.
Emex: I’m tired of being told suffering is acceptable because the future will be perfect.
Emex: I want people who can say, “this hurts,” without turning it into doctrine.

Emex: <b>Luminar</b> talks about rebuilding the world.
Emex: <b>Solace</b> talks about rebuilding people.
-> a_menu


=== hub_b ===
Emex: So you met <b>Marrek</b>.
Emex: Hmmm… So you are one of them now.

Emex: Don’t worry. That doesn’t mean you’re lost.
Emex: It just means you stopped being impressed by speeches.

-> b_menu

=== b_menu ===
+ [Leave.] -> END
+ [What do you think the scholars are hiding?] -> b1
+ [What about the Revivalists???] -> b2
+ [What should I watch out for in Luminar?] -> b3

=== b1 ===
Emex: I don’t have proof.
Emex: But I’ve watched them dodge the same questions the same way—too polished.
Emex: And I’ve watched <b>Sanya</b> turn uncertainty into confidence like it’s a magic trick.

Emex: If you keep pushing, they’ll stop smiling.
Emex: That’s when you’ll learn what “hope” looks like when it gets cornered.
-> b_menu

=== b2 ===
Emex: They’re not saints.
Emex: They’re rough, but they’re honest about what they are.
Emex: <b>Scholars</b> talk like they’re saving everyone… even when nobody asked to be saved.

Emex: <b>Solace</b> will tell you “no” to your face.
Emex: <b>Luminar</b> will tell you “yes” and then decide what you’re allowed to hear.
-> b_menu

=== b3 ===
Emex: The way they praise you.
Emex: The way they recruit you.
Emex: They don’t want you to be a person. They want you to be useful.

Emex: Enjoy the kindness—just don’t confuse it with safety.
Emex: And don’t let them make your questions feel childish.
-> b_menu