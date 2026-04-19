INCLUDE globals.ink
-> start

=== start ===
~ register_npc(NPC.julisa)

{ spoke_to_marrek:
    -> hub_b
- else:
    -> hub_a
}

=== hub_a ===
Julisa: Oh—hello!
Julisa: Sorry—hi. You startled me.
Julisa: You’re the explorer, right? The real one?
Julisa: Not “I walked a mile outside the fence and got brave.” I mean *out there.*

-> a_menu

=== a_menu ===
+ [Leave.] -> END
+ [What are you researching?] -> a1
+ [What powered the old world?] -> a2
+ [Do you know how I crashed?] -> a3

=== a1 ===
Julisa: The shutdown. The planet-wide silence.
Julisa: Every surviving text agrees on one thing:
Julisa: a single power source. A single heartbeat in the world.

Julisa: Isn’t that terrifying?
Julisa: One heartbeat—one failure—everything goes dark.
Julisa: The scholars call it tragedy. I call it a clue.

-> a_menu

=== a2 ===
Julisa: My leading theory? The ancients drew power from the sun.
Julisa: Not like panels—something deeper. Something *engineered.*
Julisa: I know it sounds impossible.
Julisa: But the old age was built on “impossible.”

Julisa: And when your whole world runs on impossible…
Julisa: you don’t get small failures.
Julisa: You get total silence.

-> a_menu

=== a3 ===
Julisa: I… don’t know.
Julisa: Not in the clean, satisfying way you want.
Julisa: Some travelers say it’s bad luck.
Julisa: Some say it’s the planet “collecting” people.

Julisa: The scholars won’t say that out loud.
Julisa: But if you want my honest answer?
Julisa: I think there’s a pattern we haven’t earned the right to prove yet.

-> a_menu


=== hub_b ===
Julisa: You’ve been around Solace.
Julisa: Okay—then you’ve heard a lot of confident opinions with very little evidence.
Julisa: They sound certain because certainty feels safe.

-> b_menu

=== b_menu ===
+ [Leave.] -> END
+ [What do the scholars believe caused the shutdown?] -> b1
+ [Is your “sun theory” still your best guess?] -> b2
+ [Did anything unusual appear in the records about outsiders?] -> b3

=== b1 ===
Julisa: We don’t pretend to know everything.
Julisa: But we see the pattern: one backbone system, one failure, total silence.
Julisa: Solace calls it “a warning.” We call it “a problem to solve.”

Julisa: Same facts. Different instinct.
Julisa: They flinch away from the past.
Julisa: We walk toward it with tools.

-> b_menu

=== b2 ===
Julisa: It’s still a contender.
Julisa: But now I’m less sure it’s the *sun* and more sure it’s a *single regulator*—
Julisa: something that distributed power.
Julisa: The texts use words like “flow,” “balance,” “heartbeat.”

Julisa: That doesn’t sound like random sunlight.
Julisa: That sounds like infrastructure.
Julisa: That sounds like a machine with a job… and a failure.

-> b_menu

=== b3 ===
Julisa: Records mention travelers… but not clearly.
Julisa: It’s like the texts get vague on purpose.
Julisa: Like someone edited history with a blunt tool.

Julisa: When I ask Sanya, she smiles and says, “focus on what we can prove.”
Julisa: Which is… polite.
Julisa: And also a little terrifying.

-> b_menu