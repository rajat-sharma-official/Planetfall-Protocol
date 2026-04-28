INCLUDE globals.ink
A harsh metallic screech cuts through the clearing.

A ruined ship sits half-canted in the dirt, its hull split like a cracked shell.

An old scrapper is already inside it, wrenching at a scorched panel with practiced force.

The metal tears loose.

CLANG.

He glances over his shoulder at you.

Grald: You're upright.

His gaze moves over you once, then to VERA, then back to his work.

Grald: Great.
Grald: Now I have an audience.

-> main_hub


=== main_hub
~register_npc(NPC.grald)

+ [I should get going.]
    -> done

+ [What is this wreck?]
    Grald doesn't stop working.

    Grald: A ship that lost an argument with the sky.
    Grald: Like most of them.

    He tugs another piece free.

    Grald: If you're hoping for a tragic story, go find someone who still cares.
    -> ownership_hub

+ [Get away from it.]
    Grald pauses just long enough to look at you properly.

    Grald: No.

    Beat.

    Grald: Say it again if it makes you feel taller.
    -> ownership_hub

+ [What are you doing?]
    Grald lifts a cracked coupling into the light, inspects it, then tosses it aside.

    Grald: Sorting.
    Grald: Most wrecks are ninety percent garbage dressed up as something important.
    Grald: Burnt plating. Dead boards. Junk with a dramatic finish.

    He reaches back into the ship.

    Grald: I <b>keep the parts</b> that still have a reason to exist.
    -> mechanic_hub



=== ownership_hub
+ [I should get going.]
    -> done

+ [Whose ship was this?]
    Grald finally pulls himself halfway out of the hull.

    Grald: Someone who isn't here to argue about it.

    He plants the prybar against his shoulder.

    Grald: That's usually how ownership ends out here.
    -> ownership_hub

+ [So you loot crash sites?]
    Grald gives you a look that suggests the question embarrassed him on your behalf.

    Grald: No.
    Grald: I arrive before weather, rust, and idiots finish wasting what's left.

    A pause.

    Grald: If you want to moralize, do it somewhere softer.
    -> ownership_hub

+ [Why are there so many wrecks?]
    Grald's eyes narrow slightly.

    Grald: Because the sky here <b>doesn't forgive mistakes.</b>
    Grald: And because people keep flying anyway.

    He points into the open hull.

    Grald: Curiosity, greed, desperation.
    Grald: Pick your poison.
    -> mechanic_hub



=== mechanic_hub
+ [I should get going.]
    -> done

+ [What parts are worth taking?]
    Grald points with two dirty fingers.

    Grald: Stored <b>charge.</b>
    Grald: <b>Control hardware.</b>
    Grald: <b>Connectors</b> that aren't fused to hell.

    He nudges a bent panel with his boot.

    Grald: The rest is weight.
    -> mechanic_hub

+ [What's the best way to scavenge?]
    Grald studies you for a moment like he's deciding whether to bother.

    Grald: Stop grabbing big pieces.
    Grald: Big pieces make people feel productive.

    He flicks a small component into his palm.

    Grald: <b>Small parts</b> are what actually get things running again.
    -> mechanic_hub

+ [So where do I start?]
    Grald jerks his chin toward the <b>ground around the wreck.</b>

    Grald: With your eyes.
    Grald: Then with your hands.
    Grald: In that order.

    His gaze slides to <b>VERA.</b>

    Grald: Let her point out what you're staring at.
    Grald: Since you're clearly committed to missing things.
    -> vera_hub



=== vera_hub
+ [I should get going.]
    -> done

+ [What do you mean, let her point things out?]
    Grald looks at VERA, then back at you.

    Grald: I mean <b>she tracks what you look at.</b>
    Grald: So stop looking at useless things.

    He gestures toward the wreck and the debris around it.

    Grald: Open panels.
    Grald: Loose <b>scrap.</b>
    Grald: <b>Hazards.</b>
    Grald: Anything half-buried that wants to ruin your day.
    -> vera_hub

+ [What should I be looking for?]
    Grald sweeps the prybar toward the debris field.

    Grald: Loose <b>scrap</b> first.
    Grald: Then intact components.
    Grald: Then anything unstable enough to cut, shock, or collapse.

    He lowers the prybar.

    Grald: If it looks obvious, look anyway.
    Grald: Obvious is how most people die.
    -> vera_hub

+ [What should I point my attention at first?]
    Grald answers without hesitation.

    Grald: Edges.
    Grald: Open seams.
    Grald: The ground where parts roll and hide.

    He taps the hull.

    Grald: Then anything that looks too clean, too bent, or too quiet.
    -> player_hub



=== player_hub
+ [That's enough. I'll handle it.]
    -> done

+ [What do I avoid?]
    Grald scans the area in a slow circle.

    Grald: Loose footing.
    Grald: Hanging panels.
    Grald: Sharp metal.
    Grald: Anything sparking, leaking, or waiting for pressure.

    He looks back at you.

    Grald: And stop reaching into holes you can't see into.
    Grald: Unless you're trying to donate fingers to the cause.
    -> player_hub

+ [How do I know if something is valuable?]
    Grald crouches, picks up a small damaged part, then drops it again.

    Grald: If it's hard to replace.
    Grald: If it still holds shape.
    Grald: If it still fits where it belongs.

    He gives you a brief look.

    Grald: If it makes you hesitate to throw it away, it probably matters.
    -> player_hub

+ [Why are you even telling me this?]
    Grald stares at you.

    Grald: Because you're going to try anyway.
    Grald: And if you die out here, you'll make a mess where I work.

    He turns slightly back toward the wreck.

    Grald: Consider it self-interest.
    -> player_hub



=== done
Grald steps back into the wreck, already done with you.

Grald: Good.

He doesn't look at you when he speaks.

Grald: Watch your hands.
Grald: Pay attention when <b>VERA</b> flags something in your sightline.
Grald: And don't confuse a wreck with a miracle.

A pause.

Grald: If you survive the day, try to be less in the way tomorrow.

-> END