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
Harvel: These fragments are older than your ship and twice as temperamental.
-> a_menu

=== a_menu ===
+ [Leave.] -> END
+ [What are you working on?] -> a1
+ [Why is this tech so fragile?] -> a2
+ [Can this help repair my ship?] -> a3

=== a1 ===
Harvel: Directive runes. Material-etched logic.
Harvel: The Aurelians didn’t store instructions in computers.
Harvel: They carved instructions into the thing itself.
-> a_menu

=== a2 ===
Harvel: “Fragile” isn’t the word.
Harvel: Complex systems demand precision. Outsiders call that fragility because they lack patience.
Harvel: If you treat it like junk, it becomes junk.
-> a_menu

=== a3 ===
Harvel: In theory? Yes.
Harvel: If you can read the runes and understand function mapping, you can redirect behavior.
Harvel: In practice? You’ll break three parts before you learn to break fewer.
-> a_menu


=== hub_b ===
Harvel: You’ve been listening to Revivalist warnings.
Harvel: Let me guess—now everything is a “danger” and no one should build anything.
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
-> b_menu

=== b2 ===
Harvel: I don’t know.
Harvel: If you want a comforting lie, go ask someone who sells comfort.
Harvel: What I *do* know is this: Aurelia has patterns we can’t fully chart yet.
-> b_menu

=== b3 ===
Harvel: Because they refuse tools and then celebrate suffering as virtue.
Harvel: They act like survival is the same as progress.
Harvel: It’s not cruelty to say they’re stuck. It’s accuracy.
-> b_menu