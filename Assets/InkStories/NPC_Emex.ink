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
Emex: You still think Luminar smiles because it cares.
-> a_menu

=== a_menu ===
+ [Leave.] -> END
+ [Do you hate them?] -> a1
+ [Are the scholars lying to me?] -> a2
+ [Where are you going?] -> a3

=== a1 ===
Emex: Because the place is too bright.
Luminar shines with knowledge and hope—so bright everyone in it is blind.
If you don’t worship the mission, you’re treated like a defect.
-> a_menu

=== a2 ===
Emex: They’re not liars in the simple way.
They believe their story so hard they stop seeing what doesn’t fit.
And when you ask the wrong question, they smile and say, “I don’t know.”
-> a_menu

=== a3 ===
Emex: Solace.
I’m tired of being told suffering is acceptable because the future will be perfect.
I want people who can say, “this hurts,” without turning it into doctrine.
-> a_menu


=== hub_b ===
Emex: So you met Marrek.
hmmm... So you are one of them now
-> b_menu

=== b_menu ===
+ [Leave.] -> END
+ [What do you think the scholars are hiding?] -> b1
+ [What about the Revivalists???] -> b2
+ [What should I watch out for in Luminar?] -> b3

=== b1 ===
Emex: I don’t have proof.
But I’ve watched them dodge the same questions the same way—too polished.
And I’ve watched Sanya turn uncertainty into confidence like it’s a magic trick.
If you keep pushing, they’ll stop smiling.
-> b_menu

=== b2 ===
Emex: They're all the same bunch in my eyes.
They’re rough, but they’re honest about what they are.
Scholars talk like they’re saving everyone… even when nobody asked to be saved.
-> b_menu

=== b3 ===
Emex: The way they praise you.
The way they recruit you.
They don’t want you to be a person. They want you to be useful.
Enjoy the kindness—just don’t confuse it with safety.
-> b_menu