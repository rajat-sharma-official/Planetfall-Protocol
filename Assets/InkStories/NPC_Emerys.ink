INCLUDE globals.ink

Emerys is kneeling beside a scattered circle of metal tablets when you approach.

Each fragment is covered in strange carved markings.

Emerys: ...no, no, that symbol repeats after the fourth line...

Emerys pauses.

Emerys: ...unless the sequence folds.

Emerys finally notices you standing there.

Emerys: Oh.

Emerys: Hello.

Emerys: Sorry.
Emerys: I get lost in these things.

-> main_hub


=== main_hub
~register_npc(NPC.emerys)

+ [I should get going.]
    -> done

+ [Who are you?]
    Emerys blinks.

    Emerys: Right.
    Emerys: Introductions.

    Emerys brushes dust from their hands.

    Emerys: Emerys.
    Emerys: Archivist.

    Emerys gestures to the fragments scattered around them.

    Emerys: I collect pieces of the past before they vanish completely.

    Emerys pauses.

    Emerys: ...did that answer the question?

    -> main_hub

+ [I'm traveling with an AI named VERA.]
    Emerys freezes.

    Emerys: An AI?

    Emerys suddenly becomes intensely curious.

    Emerys: Is it operational?
    Emerys: Does it process symbolic languages?
    Emerys: Does it—

    Emerys stops abruptly.

    Emerys: Sorry.

    Emerys: I forget not everyone enjoys interrogation.

    -> archive_hub

+ [What exactly are you studying?]
    Emerys picks up a metal tablet carefully.

    Emerys: Aurelian records.

    Emerys taps the etched symbols.

    Emerys: They carved knowledge directly into their materials.

    Emerys pauses.

    Emerys: Stone survives.
    Emerys: Metal survives.
    Emerys: Paper does not.

    -> archive_hub


=== archive_hub
+ [I should get going.]
    -> done

+ [Can you actually read those symbols?]
    Emerys: Fragments.

    Emerys: Imagine finding a book where every third page is missing.

    Emerys gestures toward the tablets.

    Emerys: That's what this is like.

    Emerys pauses.

    Emerys: ...actually that's what most history is like.

    -> archive_hub

+ [Why preserve these records at all?]
    Emerys considers the question.

    Emerys: Because someone tried very hard to write them.
    Emerys: Civilizations collapse.
    Emerys: But knowledge doesn't have to.

    Emerys taps the metal fragment again.

    Emerys: If someone remembers to look.

    -> archive_hub

+ [Have you discovered anything interesting?]
    Emerys glances down at the fragments.

    Emerys: Patterns.
    Emerys: Repeating structures.
    Emerys: Warnings.

    Emerys pauses.

    Emerys: ...and something about containment.

    Emerys frowns slightly.

    Emerys: That part concerns me.

    -> mystery_hub


=== mystery_hub
+ [Alright, that's enough questions.]
    -> done

+ [What do the records say about Aurelia?]
    Emerys looks thoughtful.

    Emerys: Not enough.
    Emerys: Most of the records survived.
    Emerys: The explanations did not.

    Emerys pauses.

    Emerys: Which is inconvenient.

    -> mystery_hub

+ [Do the records say anything about the storms?]
    Emerys nods slightly.

    Emerys: Yes.
    Emerys: But they don't call them storms.
    Emerys: They describe disturbances in the sky.

    Emerys glances upward.

    Emerys: Instability.
    Emerys: ...which is not comforting.

    -> mystery_hub

+ [Do the records mention travelers like me?]
    Emerys pauses.

    Emerys: A few references.
    Emerys: Visitors.
    Emerys: Arrivals.
    Emerys: Witnesses.

    Emerys frowns slightly.

    Emerys: They seemed to expect someone to come.

    -> mystery_hub


=== done
Emerys is already studying the tablets again.

Emerys: ...wait.
Emerys: ...what was I saying?

-> END