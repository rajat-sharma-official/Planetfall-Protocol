//NPC_Child.ink
INCLUDE globals.ink

-> start

=== start ===
~ register_npc(NPC.child)
~ met_child = true

Child: Oh! You can see me?

-> hub

=== hub ===
+ [Goodbye.] -> END
+ [Are you okay?] -> okay
+ [Are you lost?] -> maybe
+ [Who are you?] -> lost

=== lost ===
Child: I'm just... <b>lost.</b>
-> hub

=== okay ===
Child: I'm scared, but I'm okay.
-> hub

=== maybe ===
Child: ....
-> hub