INCLUDE globals.ink
-> start

=== start ===
~ register_npc(NPC.camla)

{ spoke_to_marrek:
    -> hub_b
- else:
    -> hub_a
}

=== hub_a ===
Camla: Hi! Sorry—hi.
Camla: I’ve never met someone who’s actually lived among the stars.

Camla: You’re Atlas, right?
Camla: You have that… “I’ve seen too much sky” look.

-> a_menu

=== a_menu ===
+ [Leave.] -> END
+ [What do you want to know?] -> a1
+ [Why did you join the scholars?] -> a2
+ [What do you think Aurelia used to be?] -> a3

=== a1 ===
Camla: Everything.
Camla: What’s it like when the sky isn’t just… weather?
Camla: Do you ever get tired of seeing new worlds?

Camla: And—okay—stupid question:
Camla: Do the stars ever feel lonely?
-> a_menu

=== a2 ===
Camla: Because I don’t want my life to be just surviving.
Camla: <b>Sanya</b> says we can build a world where people don’t fear winter, hunger, sickness…
Camla: I want that. Even if it takes my whole life.

Camla: The <b>Revivalists</b> talk like pain is a teacher.
Camla: I think pain is just… pain.
-> a_menu

=== a3 ===
Camla: Beautiful. Loud. Bright.
Camla: The <b>ruins</b> feel like a song that got cut off mid-note.
Camla: I want to hear the rest of it.

Camla: Sometimes I stand near the <b>broken machines</b> and imagine what they sounded like.
Camla: Like the <b>planet used to hum</b>.
-> a_menu


=== hub_b ===
Camla: You’ve been to <b>Solace</b>, haven’t you?
Camla: They look at the ruins like they’re poison.
Camla: And… I can’t tell if they’re wrong.

-> b_menu

=== b_menu ===
+ [Leave.] -> END
+ [Do you ever doubt Sanya’s mission?] -> b1
+ [What would “restoring the old age” really mean?] -> b2
+ [Why do scholars act so sure of themselves?] -> b3

=== b1 ===
Camla: NEVER—
Camla: …I mean.
Camla: It’s not that I doubt her.
Camla: It’s that sometimes I get scared of how sure she sounds.

Camla: When someone speaks like they can’t be wrong,
Camla: it makes you wonder what they do with questions.
-> b_menu

=== b2 ===
Camla: It means light at night. Clean water.
Camla: Tools that don’t break after one season.
Camla: It means nobody has to die because they slipped on a rock and no one could help.

Camla: It means people can stop living like the world is punishing them.
Camla: That’s what I want.
-> b_menu

=== b3 ===
Camla: Because if we sound unsure, people stop believing.
Camla: And if people stop believing, the outpost stops working.

Camla: Certainty is… contagious.
Camla: Sanya knows that.
-> b_menu