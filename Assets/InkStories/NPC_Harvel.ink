INCLUDE globals.ink
-> start

=== start ===
~ register_npc(NPC.harvel)

{ spoke_to_marrek:
    -> hub_b
- else:
    -> hub_a
}

=== hub_a ===
Harvel: Don’t touch anything unless you want to ruin it.
Harvel: These <b>fragments</b> are older than your ship and twice as temperamental.

Harvel: People see “scrap.”
Harvel: I see systems that outlived their makers.

-> a_menu

=== a_menu ===
+ [Leave.] -> END
+ [What are you working on?] -> a1
+ [Why is this tech so fragile?] -> a2
+ [Can this help repair my ship?] -> a3

=== a1 ===
Harvel: Directive <b>runes</b>. Material-etched logic.
Harvel: The Aurelians didn’t store instructions in computers.
Harvel: They carved instructions into the thing itself.

Harvel: That’s why it still “works” after centuries.
Harvel: And that’s why careless hands destroy it.
-> a_menu

=== a2 ===
Harvel: “Fragile” isn’t the word.
Harvel: Complex systems demand precision.
Harvel: Outsiders call that fragility because they lack patience.

Harvel: If you treat it like junk, it becomes junk.
Harvel: If you respect it, it becomes a tool again.
-> a_menu

=== a3 ===
Harvel: In theory? Yes.
Harvel: If you can read the runes and understand function mapping, you can redirect behavior.
Harvel: In practice? You’ll break three parts before you learn to break fewer.

Harvel: Start small.
Harvel: <b>Collect</b> clean plates, intact joints, anything with stable markings.
Harvel: Don’t bring me twisted metal and ask for miracles.
-> a_menu


=== hub_b ===
Harvel: You’ve been listening to Revivalist warnings.
Harvel: Let me guess—now everything is a “danger” and no one should build anything.

Harvel: Next you’ll tell me the planet is flat.
Harvel: Or that gravity is a rumor.

-> b_menu

=== b_menu ===
+ [Leave.] -> END
+ [Do the scholars hide information from outsiders?] -> b1
+ [Did scholar experiments cause my crash?] -> b2
+ [Why do you treat Solace like they’re helpless?] -> b3

=== b1 ===
Harvel: We don’t “hide.” We prioritize.
Harvel: Half-informed people cause full disasters.
Harvel: You want answers? Prove you can handle precision.

Harvel: If you can’t even keep your hands steady around a relic,
Harvel: you have no business hearing what it can do.
-> b_menu

=== b2 ===
Harvel: I don’t know.
Harvel: And if you want a comforting lie, go ask someone who sells comfort.

Harvel: What I *do* know is this:
Harvel: Aurelia has <b>patterns</b> we can’t fully chart yet.
Harvel: If you’re looking for a villain, you’ll find one whether it exists or not.
-> b_menu

=== b3 ===
Harvel: Because they refuse tools and then celebrate suffering as virtue.
Harvel: They act like survival is the same as progress.
Harvel: It’s not cruelty to say they’re stuck. It’s accuracy.

Harvel: We don’t hate them.
Harvel: We just… don’t let them steer the future while they’re afraid of it.
-> b_menu