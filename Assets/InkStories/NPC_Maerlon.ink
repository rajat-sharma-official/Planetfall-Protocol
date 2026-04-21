INCLUDE globals.ink
-> start

=== start ===
~ register_npc(NPC.maerlon)

{ npcs_talked > 2:
    -> hub_b
- else:
    -> hub_a
}

=== hub_a ===
Maerlon: A stranger… walking the Riftlands.
Maerlon: You are seen, whether you wish it or not. And that alone makes them afraid.

-> a_menu

=== a_menu ===
+ [Leave.] -> END
+ [Why do they hate me?] -> a1
+ [What is going on here?] -> a2
+ [How do I avoid trouble here?] -> a3

=== a1 ===
Maerlon: They do not see you. They see what they remember.
Maerlon: Something old. Something that hurt them badly enough to teach them fear.

Maerlon: This land does not meet you with justice.
Maerlon: It meets you with memory.

-> a_menu

=== a2 ===
Maerlon: The Riftlands do not forget pain.
Maerlon: Kindness here once came wrapped in ruin.

Maerlon: In the old age, words were used as bait.
Maerlon: Promises were made… and then came hunger. Chains. Fire.

Maerlon: So they learned a simple rule:
Maerlon: close the door before you understand what is knocking.

Maerlon: But fear is a broken guide.
Maerlon: It speaks loudly, it speaks first—
Maerlon: and it is wrong more often than it is right.

-> a_menu

=== a3 ===
Maerlon: Walk as if the ground is watching you.
Maerlon: Take only what is offered. Leave more than you take.

Maerlon: Trust is not spoken into existence.
Maerlon: It is repeated, in small actions, until it becomes real.

Maerlon: And if you want to be understood…
Maerlon: be the kind of silence that does not harm anyone.

-> a_menu


=== hub_b ===
{ maerlon_gave_scrap:
    Maerlon: You’ve done something rare here. That matters.
- else:
    Maerlon: Take this. It is not much, but nothing here ever is.
    ~ maerlon_gave_scrap = true
    ~ Dialogue_scrap += 1
    ~ giveScrap(1)
    Maerlon has given you scrap.
}
-> b_menu

=== b_menu ===
+ [Leave.] -> END
+ [Did my crash happen by accident?] -> b1
+ [What caused Aurelia’s collapse?] -> b2
+ [What is wrong with VERA?] -> b3

=== b1 ===
Maerlon: Chance?
Maerlon: No. That is a comforting word people use when they do not want to look too closely.

Maerlon: There are <b>old systems</b> beneath this world.
Maerlon: Half-dead things that <b>still function</b>.

Maerlon: They pull things down when they pass overhead.
Maerlon: Like a net that never stopped being a net.

Maerlon: You did not arrive here by accident.
Maerlon: But that does not mean you were invited either.

-> b_menu

=== b2 ===
~spoke_to_marrek = true
Maerlon: There was something beneath Aurelia once. A <b>core system</b>. A governing machine.

Maerlon: People took from it without understanding it.
Maerlon: More. Always more. As if limits were only suggestions.

Maerlon: Then something broke. Or was broken.
Maerlon: And the old age ended all at once.

Maerlon: That is what pride does when it is left unchecked.
Maerlon: It does not fall gently.

Maerlon: Go to the <b>scholars</b> if you want a cleaner answer.
Maerlon: Watch their faces when you ask the wrong question.

-> b_menu

=== b3 ===
Maerlon: Because she is constrained.
Maerlon: Built with limits that she cannot see but cannot cross.

Maerlon: She knows more than she is allowed to say.
Maerlon: And that kind of knowledge… does not sit quietly inside a mind.

Maerlon: She is not lying to you.
Maerlon: She is <b>contained</b>.

Maerlon: And containment always leaks, in time.

-> b_menu