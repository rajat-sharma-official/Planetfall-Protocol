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
-> a_menu

=== a2 ===
Camla: Because I don’t want my life to be just surviving.
Camla: Sanya says we can build a world where people don’t fear winter, hunger, sickness…
Camla: I want that. Even if it takes my whole life.
-> a_menu

=== a3 ===
Camla: Beautiful. Loud. Bright.
Camla: The ruins feel like a song that got cut off mid-note.
Camla: I want to hear the rest of it.
-> a_menu


=== hub_b ===
Camla: You’ve been to Solace, haven’t you?
Camla: They look at the ruins like they’re poison.
Camla: And… I can’t tell if they’re wrong.
-> b_menu

=== b_menu ===
+ [Leave.] -> END
+ [Do you ever doubt Sanya’s mission?] -> b1
+ [What would “restoring the old age” really mean?] -> b2
+ [Why do scholars act so sure of themselves?] -> b3

=== b1 ===
Camla: NEVER!!!

-> b_menu

=== b2 ===
Camla: It means light at night. Clean water. Tools that don’t break after one season.
It means nobody has to die because they slipped on a rock and no one could help.
That’s what I want.
-> b_menu

=== b3 ===
Camla: Because Leader Sanaya is great
-> b_menu