INCLUDE globals.ink

You approach Emerys, who kneels over a circle of carved metal fragments.

Emerys: ...no, no, that symbol repeats after the fourth line...

Emerys: ...unless the sequence folds.

Emerys finally notices you standing there.

Emerys: Oh. Hello. Sorry. I get lost in these things.

-> main_hub


=== main_hub
~register_npc(NPC.emerys)

+ [I should get going.]
    -> done

+ [Who are you?]
    Emerys: Right. Introductions.

    Emerys brushes dust from their hands, and gestures to the fragments scattered around them.

    Emerys: Emerys.<b>Archivist.</b>

    Emerys: I collect <b>pieces of the past</b> before they vanish completely.

    Emerys: ...did that answer the question?

    -> main_hub

+ [I'm traveling with an AI named VERA.]
    Emerys freezes.

    Emerys: An AI?

    Emerys suddenly becomes intensely curious.

    Emerys: Is it operational?
    Emerys: Does it process <b>symbolic languages?</b>
    Emerys: Does it—

    Emerys stops abruptly.

    Emerys: Sorry.

    Emerys: I forget not everyone enjoys interrogation.

    -> archive_hub

+ [What exactly are you studying?]
    Emerys picks up a metal tablet carefully.

    Emerys: Aurelian records.

    Emerys taps the <b>etched symbols.</b>

    Emerys: They carved knowledge directly into their materials.

    Emerys pauses.

    Emerys: Stone survives. Metal survives. Paper does not.

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

    Emerys: ...and something about <b>containment.</b>

    Emerys frowns slightly.

    Emerys: That part concerns me.

    -> mystery_hub


=== mystery_hub
+ [Alright, that's enough questions.]
    -> done

+ [What do the records say about Aurelia?]
    Emerys looks thoughtful.

    Emerys: Not enough. Most of the records survived. The explanations did not.

    Emerys pauses.

    Emerys: Which is inconvenient.

    -> mystery_hub

+ [Do the records say anything about the storms?]
    Emerys nods slightly.

    Emerys: Yes. But they don't call them storms. They describe <b>disturbances in the sky.</b>

    Emerys glances upward.

    Emerys: Instability.
    Emerys: ...which is not comforting.

    -> mystery_hub

+ [Do the records mention travelers like me?]
    Emerys pauses.

    Emerys: A few references. Visitors. Arrivals. Witnesses.

    Emerys frowns slightly.

    Emerys: They seemed to <b>expect someone to come.</b>

    -> mystery_hub


=== done
Emerys is already studying the tablets again.

Emerys: ...wait.
Emerys: ...what was I saying?

-> END